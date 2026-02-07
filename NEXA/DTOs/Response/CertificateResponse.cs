namespace NEXA.DTOs.Response
{
    public class CertificateResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public DateTime IssuedAt { get; set; }
    }
}
