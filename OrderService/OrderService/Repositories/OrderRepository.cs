using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OrderService.Models;

namespace OrderService.Repositories
{
    public class OrderRepository
    {
        private readonly IMongoCollection<Order> _ordersCollection;

        public OrderRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);

            _ordersCollection = mongoDatabase.GetCollection<Order>("Orders");
        }

        public async Task<List<Order>> GetAsync() =>
            await _ordersCollection.Find(_ => true).ToListAsync();

        public async Task<Order> GetAsync(string id) =>
            await _ordersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order newOrder) =>
            await _ordersCollection.InsertOneAsync(newOrder);

        public async Task UpdateAsync(string id, Order updatedOrder) =>
            await _ordersCollection.ReplaceOneAsync(x => x.Id == id, updatedOrder);

        public async Task RemoveAsync(string id) =>
            await _ordersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
