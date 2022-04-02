using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Saga
{
    public interface IOrderNotFoundEvent
    {
        Guid OrderId { get; }

        string Message { get; }
    }
}
