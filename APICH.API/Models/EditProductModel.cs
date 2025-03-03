namespace APICH.API.Models
{
    public class EditProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile? Image { get; set; }
        public string Token { get; set; }
    }
}
