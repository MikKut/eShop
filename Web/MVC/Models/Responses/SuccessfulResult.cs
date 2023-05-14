namespace MVC.Models.Responses
{
    public record SuccessfulResultResponse
    {
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
