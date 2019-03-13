using EventBusRabbitMQ.Events;

namespace OrderingMicroS.Events
{
    public class OrderGetAllIntegrationEvent : IntegrationEvent
    {
        public long ProductId { get; set; }
    }
}
