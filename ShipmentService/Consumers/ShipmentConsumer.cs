using MassTransit;
using SharedLogic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipmentService.Consumers
{
    public class ShipmentConsumer : IConsumer<IShipmentCommand>
    {
        public Task Consume(ConsumeContext<IShipmentCommand> context)
        {
            var message = context.Message;

            // make the shipment ready..

            return Task.CompletedTask;
        }
    }
}
