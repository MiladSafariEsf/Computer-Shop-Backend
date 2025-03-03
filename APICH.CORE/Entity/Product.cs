using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public List<Reviews> Reviews { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
