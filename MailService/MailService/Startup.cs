using MailService.Email;
using MailService.Kafka;
using MailService.Services;

namespace MailService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var kafkaBootstrapServers = _configuration["Kafka:BootstrapServers"];
            var topic = _configuration["Kafka:Topic"];

            var smtpServer = _configuration["Smtp:Server"];
            var smtpPort = Convert.ToInt32(_configuration["Smtp:Port"]);
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var fromEmail = _configuration["Smtp:FromEmail"];

            services.AddSingleton<IEmailService>(new EmailService(smtpServer, smtpPort, smtpUser, smtpPass, fromEmail));
            services.AddSingleton(new KafkaConsumer(kafkaBootstrapServers, topic, services.BuildServiceProvider().GetService<IEmailService>()));
            services.AddHostedService<KafkaConsumerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

}
