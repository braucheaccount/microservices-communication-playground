using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Commands
{
    public interface IReserveProductCommand
    {
        Guid CorrelationId { get; }

        IEnumerable<Guid> ProductIds { get; }
    }
}
