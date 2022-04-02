using SharedLogic.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Models
{
    public class CheckOrderStatusResponse : ICheckOrderStatus
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public List<Guid> ProductIds { get; set; }

        public string Status { get; set; }
    }
}
