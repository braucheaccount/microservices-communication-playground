using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Saga
{
    public interface IProductReservedEvent
    {
        Guid CorrelationId { get; }

        IEnumerable<Guid> ProductIds { get; }
    }
}
