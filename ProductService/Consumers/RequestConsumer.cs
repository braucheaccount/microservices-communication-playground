using MassTransit;
using SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{
    public class RequestConsumer : IConsumer<RequestRequest>
    {
        private readonly ILogger<RequestConsumer> _logger;

        public RequestConsumer(ILogger<RequestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<RequestRequest> context)
        {
            var data = context.Message;

            _logger.LogInformation($"RequestConsumer Received: {data.RequestMessage}; BaseValue= {data.BaseValue}");

            var responseValue = data.BaseValue * 2;


            await context.RespondAsync(new RequestResponse
            {
                ResponseMessage = $"RequestConsumer Received: {data.RequestMessage}; BaseValue= {data.BaseValue}",
                ResponseValue = responseValue
            });
        }
    }
}
