using MassTransit;
using SharedLogic.Commands;
using SharedLogic.Models;
using SharedLogic.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.StateMachines
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        // internal
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }  


        // our data
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Guid> ProductIds { get; set; } = new();
        public string OrderStatus { get; set; }
    }

    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        // all possible states
        public State CheckIfProductCanBeReservedState { get; set; }


        public State ProductReservedState { get; set; }
        public State ProductNotReservedState { get; set; }


        public State ShipmentState { get; set; } // since we dont use a new During(ShipmentState) this is obsolete
        public State CheckOrderState { get; set; }



        public OrderStateMachine()
        {
            // Declares the property to hold the instance's state as a string (the state name
            // is stored in the property)
            InstanceState(x => x.CurrentState); 

            // for every event we need to created a new correlation
            Event(() => OrderCreatedEvent, x => x.CorrelateById(context => context.Message.OrderId));

            Event(() => ProductReservedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => ProductNotReservedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Event(() => ShipmentEvent, instance => instance.CorrelateById(context => context.Message.CorrelationId));

            Event(() => CheckOrderEvent, x =>
            {
                x.CorrelateById(context => context.Message.OrderId);

                // return not found
                x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    await context.RespondAsync(new OrderNotFoundResponse
                    {
                        OrderId = context.Message.OrderId,
                        Message = $"the order is already complete"
                    });
                }));
            });


            // the initial state
            Initially(
                When(OrderCreatedEvent) 
                    .Then(context =>
                    {
                        // here we initialize the state instance with our desired order
                        context.Saga.UserId = context.Message.UserId;
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.CreatedAt = DateTime.Now;
                        context.Saga.CorrelationId = context.Message.OrderId;

                        // add fake products 
                        context.Saga.ProductIds = new List<Guid>
                        {
                            Guid.NewGuid(),
                            Guid.NewGuid(),
                            Guid.NewGuid(),
                            Guid.NewGuid()
                        };

                        context.Saga.OrderStatus = "products added";
                    }) 
                    .TransitionTo(CheckIfProductCanBeReservedState)
                    .Send(new Uri("queue:product.order.received"), context =>
                    {
                        var command = new ReserveProductCommand(context.Saga.CorrelationId);
                        return command;
                    })
                );

            // now we are inside the 2nd stage
            During(CheckIfProductCanBeReservedState, // here we enter our 2nd stage, reserve the product and wait for..
                When(ProductReservedEvent) // either the product was reserved
                    .TransitionTo(ProductReservedState)
                    .Then(context =>
                    {
                        context.Saga.OrderStatus = "products reserved";
                    })
                    .Send(new Uri("queue:product.order.shipment"), context =>
                    {
                        // here we send a new command to shippment
                        var command = new ShipmentCommand(context.Saga.CorrelationId)
                        {
                            OrderId = context.Saga.OrderId,
                            ProductIds = context.Message.ProductIds,
                            UserId = context.Saga.UserId,
                        };

                        return command;
                    })
                    .Finalize(),

                When(ProductNotReservedEvent) // or it was not reserved
                    .TransitionTo(ProductNotReservedState)
                    .Send(new Uri("queue:product.order.failed"), context => 
                    {
                        // here we send a new command that the reservation of products failed
                        var command = new OrderFailedCommand
                        {
                            OrderId = context.Saga.OrderId,
                            ReasonOrderFailed = "reservation failed: since no products are reservated, all good"
                        };

                        return command;
                    })
                    .Finalize()
                );

            // listen always
            DuringAny(
               When(CheckOrderEvent)
                   .RespondAsync(context => context.Init<CheckOrderStatusResponse>(new CheckOrderStatusResponse
                   {
                       UserId = context.Saga.UserId,
                       OrderId = context.Saga.OrderId,
                       ProductIds = context.Saga.ProductIds,
                       Status = context.Saga.OrderStatus
                   }))
                );

            SetCompletedWhenFinalized();
        }

        
        // this event is called inside the user controller 
        public Event<IOrderCreatedEvent> OrderCreatedEvent { get; private set; }


        // 2nd stage: reserve product (this can have 2 different outgoings)
        // either the product has no quantity left or the product had enough quantity and was reserved
        public Event<IProductReservedEvent> ProductReservedEvent { get; private set; }
        public Event<IProductNotReservedEvent> ProductNotReservedEvent { get; private set; }


        // 3rd stage: shipment
        public Event<IShipmentEvent> ShipmentEvent { get; private set; }


        // extra: call anytime to check for the current status
        public Event<ICheckOrderEvent> CheckOrderEvent { get; private set; }

    }
}
