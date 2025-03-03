using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class OrderDetails : BaseEntity
    {
        public int Quantity {  get; set; }
        public decimal UnitPrice { get; set; }


        public Orders Order { get; set; }
        public Guid OrderId { get; set; }


        public Product Product { get; set; }
        public Guid ProductId { get; set; }
       
    }
}
