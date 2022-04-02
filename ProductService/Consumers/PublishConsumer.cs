using MassTransit;
using ProductService.Repositories;
using SharedLogic;
using SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{
    public class PublishConsumer : IConsumer<PubishRequest>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<PublishConsumer> _logger;

        public PublishConsumer(IProductRepository productRepository, ILogger<PublishConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PubishRequest> context)
        {
            var data = context.Message;

            _logger.LogInformation($"PublishConsumer Received: {data.PublishMessage}");

            // TODO: do something with the data


            // send request further
            var endpoint = await context.GetSendEndpoint(new Uri("rabbitmq://localhost/queue:send-example-queue"));

            await endpoint.Send(new SendRequest
            {
                SendMessage = "I was sent inside the Publish Consumer",
            });
        }
    }
}
