using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogic.Commands
{
    public class ReserveProductCommand : IReserveProductCommand
    {
        public Guid CorrelationId { get; private set; }

        public IEnumerable<Guid> ProductIds { get; set; } = new List<Guid>();

        public ReserveProductCommand(Guid correlcationId)
        {
            CorrelationId = correlcationId;
        }
    }
}
