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
        private readonly ILogger<IPublishEndpoint> _logger;

        public ReserveProductConsumer(IPublishEndpoint publishEndpoint, ILogger<IPublishEndpoint> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IReserveProductCommand> context)
        {
            var isInStock = true;
            var message = context.Message;

            _logger.LogInformation($"checking if {message.ProductIds.Count()} product are in stock");
            await Task.Delay(TimeSpan.FromSeconds(5));
            _logger.LogInformation($"checkin complete - products {(isInStock ? "" : "not")} in stock");

            if (isInStock)
            {
                await _publishEndpoint.Publish<IProductReservedEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    ProductIds = message.ProductIds
                });
            }
            else
            {
                await _publishEndpoint.Publish<IProductNotReservedEvent>(new
                {
                    CorrelationId = message.CorrelationId,
                    Problem = "not all products are in stock !"
                });
            }
            
        }
    }
}
