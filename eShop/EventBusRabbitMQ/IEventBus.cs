namespace EventBusRabbitMQ
{
    public interface IEventBus
    {
        void Publish(string @event);
    }
}
