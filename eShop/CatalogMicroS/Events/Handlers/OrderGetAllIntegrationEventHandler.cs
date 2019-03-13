using CatalogMicroS.DL;
using EventBusRabbitMQ;
using System.Threading.Tasks;

namespace CatalogMicroS.Events.Handlers
{
    public class OrderGetAllIntegrationEventHandler : IIntegrationEventHandler<OrderGetAllIntegrationEvent>
    {
        private ICatalogRepository _catalogRepository;
        private IEventBus _eventBus;

        public OrderGetAllIntegrationEventHandler(ICatalogRepository catalogRepository, IEventBus eventBus)
        {
            _catalogRepository = catalogRepository;
            _eventBus = eventBus;
        }

        public async Task Handle(OrderGetAllIntegrationEvent @event)
        {

            //TODO:publish from catalog
            //_eventBus.Publish(jsonToBe, "Order");
        }
    }
}
