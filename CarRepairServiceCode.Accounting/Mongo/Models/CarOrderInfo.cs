using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CarRepairServiceCode.Accounting.Mongo.Models
{
    public class CarOrderInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int OrderId { get; set; }
        public int CarId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderAmount { get; set; }
    }
}
