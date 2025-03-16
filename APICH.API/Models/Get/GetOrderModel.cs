namespace APICH.API.Models.Get
{
    public class GetOrderModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string UserNumber { get; set; }
        public string Address { get; set; }
        public string CreateAt { get; set; }
        public decimal totalPrice { get; set; }
        public bool IsDelivered { get; set; }
        public List<GetOrderDetailModel> Details { get; set; }
        

    }
}
