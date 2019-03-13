using EventBusRabbitMQ.Events;


namespace CatalogMicroS.Events
{
    public class OrderGetAllIntegrationEvent : IntegrationEvent
    {
        public long ProductId { get; set; }
    }
}
