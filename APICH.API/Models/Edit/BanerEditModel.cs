namespace APICH.API.Models.Edit
{
    public class BanerEditModel
    {
        public string BannerName { get; set; }
        public bool IsActive {  get; set; }
        public IFormFile? Image { get; set; }
    }
}
