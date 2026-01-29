namespace NEXA.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }

        public double CompletionPercentage { get; set; }

        public Enrollment Enrollment { get; set; } = null!;
    }
}
