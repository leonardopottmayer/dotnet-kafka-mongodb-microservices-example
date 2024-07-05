using Confluent.Kafka;
using MailService.Email;
using MailService.Models;
using Newtonsoft.Json;

namespace MailService.Kafka
{
    public class KafkaConsumer : IDisposable
    {
        private readonly IConsumer<string, string> _consumer;
        private readonly string _topic;
        private readonly IEmailService _emailService;

        public KafkaConsumer(string bootstrapServers, string topic, IEmailService emailService)
        {
            var config = new ConsumerConfig
            {
                GroupId = "order-consumer-group",
                BootstrapServers = bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<string, string>(config).Build();
            _topic = topic;
            _emailService = emailService;
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var cr = _consumer.Consume(cancellationToken);
                    var order = JsonConvert.DeserializeObject<Order>(cr.Message.Value);

                    var subject = $"Order Confirmation - {order.Id}";
                    var message = $"Dear customer, your order {order.Id} has been received. \n\n Products: {string.Join(", ", order.Products)} \n\n Total value: {order.TotalValue}";
                    await _emailService.SendEmailAsync(order.UserEmail, subject, message);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
            }
            finally
            {
                _consumer.Close();
            }
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}
