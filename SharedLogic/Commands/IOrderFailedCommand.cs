using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Commands
{
    public interface IOrderFailedCommand
    {
        Guid OrderId { get; }

        string ReasonOrderFailed { get; }
    }
}
