using CarRepairServiceCode.RequestModels.CarOrderDetail;
using System;
using System.Collections.Generic;

namespace CarRepairServiceCode.RequestModels.CarOrder
{
    public class CarOrderView
    {
        public int OrderId { get; set; }
        public int CarId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal? OrderAmount { get; set; }
        public string OrderComments { get; set; }
        public ICollection<CarOrderDetailView> CarOrderDetails { get; set; }
    }
}
