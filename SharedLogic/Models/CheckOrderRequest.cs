using SharedLogic.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Models
{
    public class CheckOrderRequest : ICheckOrderEvent
    {
        public Guid OrderId { get; set; }
    }
}
