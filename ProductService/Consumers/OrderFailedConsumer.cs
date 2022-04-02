using MassTransit;
using SharedLogic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Consumers
{
    public class OrderFailedConsumer : IConsumer<IOrderFailedCommand>
    {
        public Task Consume(ConsumeContext<IOrderFailedCommand> context)
        {
            var message = context.Message;

            // do something because the order failed?
            
            return Task.CompletedTask;
        }
    }
}
