namespace Order.API.ViewModels
{
    public class CreateOrderVM
    {
        public Guid BuyerId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
