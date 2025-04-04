namespace APICH.API.Models.Get
{
    public class GetProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public bool isAdded { get; set; }
    }
}
