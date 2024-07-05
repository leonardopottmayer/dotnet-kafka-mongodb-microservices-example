using Confluent.Kafka;
using OrderService.Models;

namespace OrderService.Kafka
{
    public class KafkaProducer : IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducer(string bootstrapServers, string topic)
        {
            var config = new ProducerConfig { BootstrapServers = bootstrapServers };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _topic = topic;
        }

        public async Task ProduceOrderAsync(Order order)
        {
            var message = new Message<string, string>
            {
                Key = order.Id,
                Value = Newtonsoft.Json.JsonConvert.SerializeObject(order)
            };

            await _producer.ProduceAsync(_topic, message);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
