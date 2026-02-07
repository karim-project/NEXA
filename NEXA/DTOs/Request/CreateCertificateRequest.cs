using System.ComponentModel.DataAnnotations;

namespace NEXA.DTOs.Request
{
    public class CreateCertificateRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }

}
