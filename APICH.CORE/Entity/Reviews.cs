using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class Reviews : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string UserNumber { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
