namespace NEXA.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public  int UserId { get; set; }
        public int CourseId { get; set; }

        public DateTime EnrolledAt { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Course Course { get; set; } = null!;

        public Progress Progress { get; set; } = null!;
    }
}
