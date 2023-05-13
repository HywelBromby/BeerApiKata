namespace PubApi.Beer.Models;

public class UpdateABeerRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal PercentageAlcoholByVolume { get; set; }
}
