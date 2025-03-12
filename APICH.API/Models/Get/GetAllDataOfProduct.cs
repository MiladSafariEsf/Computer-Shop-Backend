using APICH.CORE.Entity;

namespace APICH.API.Models.Get
{
    public class GetAllDataOfProduct
    {
        public string UserName { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public float AverageRate { get; set; }
        public List<GetAllDataOfProductComments> Reviews { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
