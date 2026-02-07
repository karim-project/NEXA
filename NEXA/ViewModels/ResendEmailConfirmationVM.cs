using System.ComponentModel.DataAnnotations;

namespace NEXA.ViewModels
{
    public class ResendEmailConfirmationVM
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

    }
}
