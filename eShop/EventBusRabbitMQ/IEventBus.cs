namespace EventBusRabbitMQ
{
    public interface IEventBus
    {
        void Publish(string @event, string routingKey);
        void Subscribe(string routingKey);
    }
}
