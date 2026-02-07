using System.ComponentModel.DataAnnotations;

namespace NEXA.DTOs.Request
{
    public class CreateCourseRequest
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
    }
}
