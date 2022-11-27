using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRepairServiceCode.RequestModels.CarOrderDetail;

namespace CarRepairServiceCode.RequestModels.CarOrder
{
    public class CarOrderRequest
    {        
        public int CarId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderAmount { get; set; }
        public string OrderComments { get; set; }
        public  ICollection<CarOrderDetailRequest> CarOrderDetails { get; set; }
    }
}
