using MailService.Kafka;

namespace MailService.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly KafkaConsumer _kafkaConsumer;

        public KafkaConsumerService(KafkaConsumer kafkaConsumer)
        {
            _kafkaConsumer = kafkaConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _kafkaConsumer.StartConsumingAsync(stoppingToken);
        }
    }
}
