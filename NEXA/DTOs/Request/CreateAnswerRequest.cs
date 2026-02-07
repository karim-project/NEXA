using System.ComponentModel.DataAnnotations;

namespace NEXA.DTOs.Request
{
    public class CreateAnswerRequest
    {
        [Required]
        public string Text { get; set; } = null!;

        public bool IsCorrect { get; set; }

        [Required]
        public int QuestionId { get; set; }
    }

}
