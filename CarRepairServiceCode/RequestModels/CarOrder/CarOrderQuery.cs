using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.RequestModels.CarOrder
{
    public class CarOrderQuery
    {
        public int? CarId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderAmount { get; set; }
        public string OrderComments { get; set; }
    }
}
