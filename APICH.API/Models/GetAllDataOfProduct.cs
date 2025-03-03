using APICH.CORE.Entity;

namespace APICH.API.Models
{
    public class GetAllDataOfProduct
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        public List<GetAllDataOfProductComments> comments { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
