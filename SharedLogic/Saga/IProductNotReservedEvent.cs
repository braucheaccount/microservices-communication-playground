using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Saga
{
    public interface IProductNotReservedEvent
    {
        Guid CorrelationId { get; }

        string Problem { get; set; }
    }
}
