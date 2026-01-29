namespace NEXA.Models
{
    public class Certificate
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public DateTime IssuedAt { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
