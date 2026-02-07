namespace NEXA.DTOs.Response
{
    public class AnswerResponse
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }

}
