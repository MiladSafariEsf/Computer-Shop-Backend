namespace APICH.API.Models
{
    public class GetOrderModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserNumber { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductNumber { get; set; }

    }
}
