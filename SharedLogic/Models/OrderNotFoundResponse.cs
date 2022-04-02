using SharedLogic.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Models
{
    public class OrderNotFoundResponse : IOrderNotFoundEvent
    {
        public Guid OrderId { get; set; }

        public string Message { get; set; }
    }
}
