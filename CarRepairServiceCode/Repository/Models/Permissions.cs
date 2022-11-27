using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Repository.Models
{
    public partial class Permissions
    {
        public int PermissionId { get; set; }
        public int PositionId { get; set; }
        public string EntityForAction { get; set; }
        public bool CreateEntry { get; set; }
        public bool ReadEntry { get; set; }
        public bool UpdateEntry { get; set; }
        public bool DeleteEntry { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual EmpPosition Position { get; set; }
    }
}
