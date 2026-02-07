using System.ComponentModel.DataAnnotations;

namespace NEXA.ViewModels
{
    public class ForgetPasswordVM
    {

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
