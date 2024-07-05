namespace OrderService.Dtos
{
    public class CreateOrderDto
    {
        public List<string> Products { get; set; }
        public decimal TotalValue { get; set; }
        public string UserEmail { get; set; }
    }
}
