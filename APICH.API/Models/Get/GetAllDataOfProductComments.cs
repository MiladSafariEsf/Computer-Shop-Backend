namespace APICH.API.Models.Get
{
    public class GetAllDataOfProductComments
    {
        public Guid Id { get; set; }
        public string date {  get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public bool IsOwner { get; set; }
    }
}
