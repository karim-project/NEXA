namespace NEXA.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExamId { get; set; }
    
        public double Score { get; set; }
        public DateTime TakenAt { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Exam Exam { get; set; } = null!;
    }
}
