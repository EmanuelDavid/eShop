using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace EventBusRabbitMQ
{
    public delegate void ProcessResult(string resultMessage);

    public class RabbitMQ : IEventBus
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private IModel _consumerChannel;
        private string _queueName;
        const string EXCHANGE_NAME = "eshop_event_bus";
        private readonly int _retryCount;
        public ProcessResult _processResult;

        public RabbitMQ(IRabbitMQPersistentConnection persistentConnection, ProcessResult processResult, string queueName = null, int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _retryCount = retryCount;
            _processResult = processResult;
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: EXCHANGE_NAME,
                                 type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                _processResult(message);

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: false,
                                 consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        public void Publish(string @event, string routingKey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: EXCHANGE_NAME,
                                    type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: EXCHANGE_NAME,
                                     routingKey: routingKey,
                                     mandatory: true,
                                     basicProperties: properties,
                                     body: body);
                });
            }
        }

        public void Subscribe(string routingKey)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueBind(queue: _queueName,
                                  exchange: EXCHANGE_NAME,
                                  routingKey: routingKey);
            }
        }
    }
}
