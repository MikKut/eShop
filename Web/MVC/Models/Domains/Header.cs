namespace MVC.Models.Domains;

public record Header
{
    public string Controller { get; init; }
    public string Text { get; init; }
}
