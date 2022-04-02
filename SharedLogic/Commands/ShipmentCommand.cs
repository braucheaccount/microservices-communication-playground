using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Commands
{
    public class ShipmentCommand : IShipmentCommand
    {
        public ShipmentCommand(Guid correlationId)
        {
            CorrelationId = correlationId;

        }

        public Guid CorrelationId { get; private set; }

        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<Guid> ProductIds { get; set; } = new List<Guid>();
    }
}
