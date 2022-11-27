using System;
using System.Collections.Generic;

#nullable disable

namespace CarRepairServiceCode.Repository.Models
{
    public partial class Client
    {
        public Client()
        {
            Cars = new HashSet<Car>();
        }

        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal? Wallet { get; set; }
        public string MobileNumber { get; set; }
        public int CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
