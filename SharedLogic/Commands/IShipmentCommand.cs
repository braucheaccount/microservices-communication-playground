using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Commands
{
    public interface IShipmentCommand
    {
        Guid CorrelationId { get; }

        Guid OrderId { get; }

        Guid UserId { get; }

        IEnumerable<Guid> ProductIds { get; }
    }
}
