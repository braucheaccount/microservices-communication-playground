using MassTransit;
using SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{
    public class ExceptionConsumer : IConsumer<ExceptionRequest>
    {
        private readonly ILogger<ExceptionConsumer> _logger;

        public ExceptionConsumer(ILogger<ExceptionConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ExceptionRequest> context)
        {
            var data = context.Message;

            _logger.LogInformation($"ExceptionConsumer Received: show exception: {data.ThrowException}");

            if(data.ThrowException)
            {
                throw new Exception("Very bad things happened");
            }

            return Task.CompletedTask;
        }
    }
}
