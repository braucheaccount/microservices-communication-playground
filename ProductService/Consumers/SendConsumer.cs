using MassTransit;
using ProductService.Repositories;
using SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{
    public class SendConsumer : IConsumer<SendRequest>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<SendConsumer> _logger;

        public SendConsumer(IProductRepository productRepository, ILogger<SendConsumer> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendRequest> context)
        {
            var data = context.Message;

            _logger.LogInformation($"SendConsumer Received: {data.SendMessage}");

            // TODO: do something with the data
        }
    }
}
