using Elfie.Serialization;

namespace NEXA.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string ContentType { get; set; } = null!; // Video / PDF

        public int ModuleId { get; set; }
        public Module Module { get; set; } = null!;

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();
    }
}
