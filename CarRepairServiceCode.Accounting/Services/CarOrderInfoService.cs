using CarRepairServiceCode.Accounting.Mongo.Models;
using CarRepairServiceCode.Accounting.Services.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CarRepairServiceCode.Accounting.Services
{
    public class CarOrderInfoService : ICarOrderInfoService
    {
        private readonly IMongoCollection<CarOrderInfo> _orderInfo;
        private readonly ILogger _logger;

        public CarOrderInfoService(IMongoDatabaseSettings settings, ILoggerFactory loggerFactory)
        {
            this._logger = loggerFactory.CreateLogger<CarOrderInfoService>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _orderInfo = database.GetCollection<CarOrderInfo>(settings.CollectionName);
        }

        public void CreateCarOrderInfo(CarOrderInfo orderInfo)
        {
            var checkOrder = GetCarOrderInfoById(orderInfo.OrderId);
            if (checkOrder != null)
                return;

            decimal amount = (decimal)orderInfo.OrderAmount; 
            orderInfo.OrderAmount = amount * 0.8M;
            _orderInfo.InsertOne(orderInfo);

            _logger.LogInformation($"CarOrderInfo created with id = {orderInfo.OrderId}");
        }

        public CarOrderInfo GetCarOrderInfoById(int id)
        {
            var orderInfo = _orderInfo.Find<CarOrderInfo>(order => order.OrderId == id).FirstOrDefault();

            return orderInfo;
        }

        public void UpdateCarOrderInfo(CarOrderInfo orderInfo)
        {
            var checkOrder = GetCarOrderInfoById(orderInfo.OrderId);
            if (checkOrder == null)
                return;

            decimal amount = (decimal)orderInfo.OrderAmount;
            orderInfo.OrderAmount = amount * 0.8M;
            orderInfo.Id = checkOrder.Id;
            _orderInfo.FindOneAndReplace(order => order.Id == checkOrder.Id, orderInfo);

            _logger.LogInformation($"CarOrderInfo updated with id = {checkOrder.Id}");
        }

        public void RemoveCarOrderInfo(CarOrderInfo orderInfo)
        {
            var checkOrder = GetCarOrderInfoById(orderInfo.OrderId);
            if (checkOrder == null)
                return;

            _orderInfo.DeleteOne(order => order.Id == checkOrder.Id);

            _logger.LogInformation($"CarOrderInfo deleted with id = {checkOrder.Id}");
        }
    }
}
