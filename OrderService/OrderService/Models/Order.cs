using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<string> Products { get; set; }
        public decimal TotalValue { get; set; }
        public string UserEmail { get; set; }
    }
}
