namespace NEXA.Models
{
    public class Exam
    {
        public  int Id { get; set; }
        public string Title { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
