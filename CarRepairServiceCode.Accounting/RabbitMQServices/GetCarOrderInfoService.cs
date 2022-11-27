using CarRepairServiceCode.Accounting.Models;
using CarRepairServiceCode.Accounting.Mongo.Models;
using CarRepairServiceCode.Accounting.RabbitMQServices.RabbitConfig;
using CarRepairServiceCode.Accounting.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarRepairServiceCode.Accounting.Services.Interfaces;

namespace CarRepairServiceCode.Accounting.RabbitMQServices
{
    public class GetCarOrderInfoService : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly ILogger _logger;
        private readonly ICarOrderInfoService _carOrderInfoService;

        public GetCarOrderInfoService(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILoggerFactory loggerFactory, ICarOrderInfoService carOrderInfoService)
        {
            _carOrderInfoService = carOrderInfoService;
            this._logger = loggerFactory.CreateLogger<GetCarOrderInfoService>();
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _carOrderInfoService = carOrderInfoService;

            InitializeRabbitMqListener();
        }

        public void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var entityModel = JsonConvert.DeserializeObject<EntityInfo<CarOrder>>(content);
                HandleMessage(entityModel?.ToString());
                ActionIdentification(entityModel);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(_queueName, true, consumer);

            return Task.CompletedTask;
        }

        private void ActionIdentification(EntityInfo<CarOrder> entityModel)
        {
            var info = CreateCarOrderInfo(entityModel.Info);
            switch (entityModel.Action)
            {
                case "create":
                    _carOrderInfoService.CreateCarOrderInfo(info);
                    break;

                case "update":
                    _carOrderInfoService.UpdateCarOrderInfo(info);
                    break;

                case "delete":
                    _carOrderInfoService.RemoveCarOrderInfo(info);
                    break;
            }
        }

        private CarOrderInfo CreateCarOrderInfo(CarOrder order)
        {
            CarOrderInfo info = new CarOrderInfo
            {
                OrderId = order.OrderId,
                CarId = order.CarId,
                OrderAmount = order.OrderAmount,
                OrderDate = order.OrderDate
            };

            return info;
        }

        private void HandleMessage(string content)
        {
            _logger.LogInformation($"consumer received {content}");
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
