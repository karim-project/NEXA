namespace NEXA.Models
{
    public class Resource
    {
        public int Id { get; set; }
        public string File { get; set; } = null!;

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; } = null!;
    }
}
