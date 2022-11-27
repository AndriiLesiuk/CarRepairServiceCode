using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Repository.Models
{
    public partial class EmpPosition
    {
        public EmpPosition()
        {
            Employees = new HashSet<Employee>();
            Permissions = new HashSet<Permissions>();
        }

        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Permissions> Permissions { get; set; }
    }
}
