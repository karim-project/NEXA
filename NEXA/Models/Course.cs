using System.Reflection;

namespace NEXA.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

     

        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}
