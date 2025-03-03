namespace APICH.API.Models
{
    public class ProductEditModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public IFormFile? image { get; set; }
    }
}
