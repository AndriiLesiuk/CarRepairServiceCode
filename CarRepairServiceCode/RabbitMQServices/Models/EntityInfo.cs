namespace CarRepairServiceCode.RabbitMQServices.Models
{
    public class EntityInfo<T>
    {
        public T Info { get; set; }
        public string Action { get; set; }
    }
}
