using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Accounting.Models
{
    public partial class CarOrder
    {
        public CarOrder()
        {
            CarOrderDetails = new HashSet<CarOrderDetail>();
        }

        public int OrderId { get; set; }
        public int CarId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? OrderAmount { get; set; }
        public string OrderComments { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Car Car { get; set; }
        public virtual ICollection<CarOrderDetail> CarOrderDetails { get; set; }
    }
}
