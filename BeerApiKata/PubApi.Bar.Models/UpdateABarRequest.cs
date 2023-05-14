namespace PubApi.Bar.Models;

public class UpdateABarRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}
