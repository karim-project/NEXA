using System.ComponentModel.DataAnnotations;

namespace NEXA.DTOs.Request
{
    public class UpdateAnswerRequest
    {
        [Required]
        public string Text { get; set; } = null!;

        public bool IsCorrect { get; set; }
    }

}
