namespace APICH.API.Models.Add
{
    public class AddBanerModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public IFormFile Image { get; set; }
    }
}
