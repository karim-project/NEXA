using System.ComponentModel.DataAnnotations;

namespace movieSystem.ViewModel
{
    public class NewPasswordVM
    {
        public int Id { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } =String.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = String.Empty;

        public string ApplicationUserId { get; set; }
    }
}
