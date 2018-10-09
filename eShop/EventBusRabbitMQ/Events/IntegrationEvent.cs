using System;

namespace Events.EventBusRabbitMQ
{
    public interface IntegrationEvent
    {
        Guid Id { get; }
        DateTime CreationDate { get; }
    }
}
