using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Accounting.Models
{
    public partial class Employee
    {
        public Employee()
        {
            CarOrderDetails = new HashSet<CarOrderDetail>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PositionId { get; set; }
        public bool IsActive { get; set; }
        public string EmpLogin { get; set; }
        public string EmpPassword { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual EmpPosition Position { get; set; }
        public virtual ICollection<CarOrderDetail> CarOrderDetails { get; set; }
    }
}
