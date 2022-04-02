using MassTransit;
using SharedLogic.Commands;
using SharedLogic.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{

    public class ReserveProductConsumer : IConsumer<IReserveProductCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ReserveProductConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IReserveProductCommand> context)
        {
            var isInStock = true;
            var message = context.Message;

            if (isInStock)
            {
                await _publishEndpoint.Publish<IProductReservedEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    ProductIds = new List<Guid>
                    {
                        Guid.NewGuid(),
                        Guid.NewGuid(),
                    }
                });
            }
            else
            {
                await _publishEndpoint.Publish<IProductNotReservedEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    Problem = "Quantity = 0 !"
                });
            }
            
        }
    }
}
