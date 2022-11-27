using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Accounting.Models
{
    public partial class Car
    {
        public Car()
        {
            CarOrders = new HashSet<CarOrder>();
        }

        public int CarId { get; set; }
        public int ClientId { get; set; }
        public string CarName { get; set; }
        public string VinNumber { get; set; }
        public string Colour { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<CarOrder> CarOrders { get; set; }
    }
}
