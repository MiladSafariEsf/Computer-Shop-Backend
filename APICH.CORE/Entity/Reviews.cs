using System.Text.Json.Serialization;

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
        public User User { get; set; }
    }
}
