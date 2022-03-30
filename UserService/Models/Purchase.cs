using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class Purchase
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public User User { get; set; }
    }
}
