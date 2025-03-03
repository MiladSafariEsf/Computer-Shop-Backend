using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public string Number {  get; set; }
        public bool IsAdmin { get; set; }
        public List<Reviews> Reviews { get; set; }
        public List<Orders> Orders { get; set; }
    }
}
