using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Repository.Models
{
    public partial class CarOrderDetail
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        
        public virtual Employee Employee { get; set; }
        public virtual CarOrder Order { get; set; }
        public virtual TaskCatalog Task { get; set; }
    }
}
