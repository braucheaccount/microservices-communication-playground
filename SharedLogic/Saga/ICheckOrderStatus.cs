using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Saga
{
    public interface ICheckOrderStatus
    {
        Guid OrderId { get; }

        Guid UserId { get; }

        List<Guid> ProductIds { get; }

        string Status { get; }
    }
}
