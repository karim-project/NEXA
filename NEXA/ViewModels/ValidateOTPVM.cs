using System.ComponentModel.DataAnnotations;

namespace NEXA.ViewModels
{
    public class ValidateOTPVM
    {
        [Required]
        public string OTP { get; set; }

        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
