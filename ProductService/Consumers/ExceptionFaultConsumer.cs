using MassTransit;
using SharedLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{
    // alternatively: IConsumer<Fault> to listen for any exception (gets everything except the exception message)
    public class ExceptionFaultConsumer : IConsumer<Fault<ExceptionRequest>> 
    {
        private readonly ILogger<ExceptionFaultConsumer> _logger;

        public ExceptionFaultConsumer(ILogger<ExceptionFaultConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Fault<ExceptionRequest>> context)
        {
            var exception = context.Message.Exceptions.FirstOrDefault();

            _logger.LogInformation($"### new exception registerd: {exception.Message}");
            

            return Task.CompletedTask;
        }
    }
}
