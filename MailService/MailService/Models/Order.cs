namespace MailService.Models
{
    public class Order
    {
        public string Id { get; set; }
        public List<string> Products { get; set; }
        public decimal TotalValue { get; set; }
        public string UserEmail { get; set; }
    }
}
