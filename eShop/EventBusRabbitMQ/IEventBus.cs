namespace EventBusRabbitMQ
{
    public interface IEventBus
    {
        void Publish(object @event, string routingKey);
        void Subscribe(string routingKey);
    }
}
