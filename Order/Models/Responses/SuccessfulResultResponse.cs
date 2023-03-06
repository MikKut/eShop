namespace Order.Host.Models.Responses
{
    public class SuccessfulResultResponse
    {
        public bool IsCompletedSuccessfully { get; set; }
        public string? Message { get; set; }
    }
}
