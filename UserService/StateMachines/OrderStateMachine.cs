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
        public Guid CorrelationId { get; set; } // used to difference between different states
        public string CurrentState { get; set; }
        // either CheckIfProductCanBeReservedState, ProductReservedState, ProductNotReservedState, ...


        // our data
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

    }

    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        // here we need to check if a product can be reserved
        public State CheckIfProductCanBeReservedState { get; set; }


        public State ProductReservedState { get; set; }
        public State ProductNotReservedState { get; set; } // for an optional During(</>) logic


        public State ShipmentState { get; set; } // since we dont use a new During(ShipmentState) this is obsolete


        public OrderStateMachine()
        {
            // Declares the property to hold the instance's state as a string (the state name
            // is stored in the property)
            InstanceState(x => x.CurrentState); 

            // for every event we need to created a new correlation
            Event(() => OrderCreatedEvent, x => x.CorrelateById(context => context.Message.OrderId).SelectId(s => Guid.NewGuid()));

            Event(() => ProductReservedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => ProductNotReservedEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

            Event(() => ShipmentEvent, instance => instance.CorrelateById(selector => selector.Message.CorrelationId));


            Initially( // the initial state
                When(OrderCreatedEvent) 
                    .Then(ProcessInitialOrderSetup) // here we initialize the state instance with our desired order
                    .TransitionTo(CheckIfProductCanBeReservedState)
                    .Send(new Uri("queue:product.order.received"), context => // here we use masstransit .send to send an event
                    {
                        var command = new ReserveProductCommand(context.Saga.CorrelationId)
                        {
                            ProductIds = new List<Guid>()
                        };
                        return command;
                    })
                );

            During(CheckIfProductCanBeReservedState, // here we enter our 2nd stage, reserve the product and wait for..
                When(ProductReservedEvent) // either the product was reserved
                    .TransitionTo(ProductReservedState)
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



            SetCompletedWhenFinalized();

        }

        
        private async void ProcessInitialOrderSetup(BehaviorContext<OrderStateInstance, IOrderCreatedEvent> context)
        {
            context.Saga.UserId = context.Message.UserId;
            context.Saga.OrderId = context.Message.OrderId;
            context.Saga.CreatedAt = DateTime.Now;
        }

        // this event is called inside the user controller 
        public Event<IOrderCreatedEvent> OrderCreatedEvent { get; private set; }


        // 2nd stage: reserve product (this can have 2 different outgoings)
        // either the product has no quantity left or the product had enough quantity and was reserved
        public Event<IProductReservedEvent> ProductReservedEvent { get; set; }
        public Event<IProductNotReservedEvent> ProductNotReservedEvent { get; set; }


        // 3rd stage: shipment
        public Event<IShipmentEvent> ShipmentEvent { get; set; }
    }
}
