using CarRepairServiceCode.RabbitMQServices.Models;

namespace CarRepairServiceCode.RabbitMQServices.Interfaces
{
    public interface IAccountingInfoSender
    {
        void SendEntityInfo<T>(EntityInfo<T> order);
    }
}
