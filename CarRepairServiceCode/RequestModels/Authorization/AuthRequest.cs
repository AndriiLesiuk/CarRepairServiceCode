using System.ComponentModel.DataAnnotations;

namespace CarRepairServiceCode.RequestModels.Authorization
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Login Field Is Empty")]
        public string EmpLogin { get; set; }

        [Required(ErrorMessage = "Password Field Is Empty")]
        [DataType(DataType.Password)]
        public string EmpPassword { get; set; }
    }
}
