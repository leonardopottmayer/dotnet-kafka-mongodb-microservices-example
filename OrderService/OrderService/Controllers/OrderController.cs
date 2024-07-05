using Microsoft.AspNetCore.Mvc;
using OrderService.Dtos;
using OrderService.Kafka;
using OrderService.Models;
using OrderService.Repositories;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly OrderRepository _orderRepository;
        private readonly KafkaProducer _kafkaProducer;

        public OrderController(ILogger<OrderController> logger, OrderRepository orderRepository, KafkaProducer kafkaProducer)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost(Name = "CreateOrder")]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto newOrderDto)
        {
            var newOrder = new Order
            {
                Products = newOrderDto.Products,
                TotalValue = newOrderDto.TotalValue,
                UserEmail = newOrderDto.UserEmail
            };

            await _orderRepository.CreateAsync(newOrder);

            await _kafkaProducer.ProduceOrderAsync(newOrder);

            return Ok();
        }
    }
}
