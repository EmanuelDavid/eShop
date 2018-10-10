using System;

namespace EventBusRabbitMQ.Events
{
    public interface IntegrationEvent
    {
        Guid Id { get; }
        DateTime CreationDate { get; }
    }
}
