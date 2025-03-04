using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APICH.CORE.Entity
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonIgnore]
        public string HashedPassword { get; set; }
        [JsonIgnore]
        public string Salt { get; set; }
        public string Number {  get; set; }
        public bool IsAdmin { get; set; }
        [JsonIgnore]
        public List<Reviews> Reviews { get; set; }
        [JsonIgnore]
        public List<Orders> Orders { get; set; }
    }
}
