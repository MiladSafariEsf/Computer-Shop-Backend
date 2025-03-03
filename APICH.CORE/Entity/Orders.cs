using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class Orders : BaseEntity
    {
        public User User { get; set; }
        public string UserNumber { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

        public DateTime CreateAt { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsDelivered { get; set; }
    }
}
