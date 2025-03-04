namespace APICH.API.Models
{
    public class AddProductRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoriesId { get; set; }
        public IFormFile Image { get; set; }
    }
}
