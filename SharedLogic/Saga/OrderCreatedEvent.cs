using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Saga
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }
    }
}
