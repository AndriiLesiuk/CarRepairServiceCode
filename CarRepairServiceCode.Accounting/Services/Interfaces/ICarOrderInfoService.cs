using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRepairServiceCode.Accounting.Mongo.Models;

namespace CarRepairServiceCode.Accounting.Services.Interfaces
{
    public interface ICarOrderInfoService
    {
        void CreateCarOrderInfo(CarOrderInfo orderInfo);
        CarOrderInfo GetCarOrderInfoById(int id);
        void UpdateCarOrderInfo(CarOrderInfo orderInfo);
        void RemoveCarOrderInfo(CarOrderInfo orderInfo);
    }
}
