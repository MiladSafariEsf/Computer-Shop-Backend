using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class Categories : BaseEntity
    {
        public string CategoryName { get; set; }
        public List<Product> Products { get; set; }
    }
}
