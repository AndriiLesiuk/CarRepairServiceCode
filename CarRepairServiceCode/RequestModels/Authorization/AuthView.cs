using System;

namespace CarRepairServiceCode.RequestModels.Authorization
{
    public class AuthView
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PositionId { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
