using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Repository.Models
{
    public partial class TaskCatalog
    {
        public TaskCatalog()
        {
            CarOrderDetails = new HashSet<CarOrderDetail>();
        }

        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? TaskPrice { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<CarOrderDetail> CarOrderDetails { get; set; }
    }
}
