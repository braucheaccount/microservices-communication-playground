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
        private readonly ILogger<ShipmentConsumer> _logger;

        public ShipmentConsumer(ILogger<ShipmentConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IShipmentCommand> context)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var data = context.Message;
            _logger.LogInformation($"shipment for {data.ProductIds.Count()} products to customer {data.UserId}");
            // make the shipment ready..
            
        }
    }
}
