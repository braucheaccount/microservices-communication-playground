using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Commands
{
    public class OrderFailedCommand : IOrderFailedCommand
    {
        public Guid OrderId { get; set; }

        public string ReasonOrderFailed { get; set; }
    }
}
