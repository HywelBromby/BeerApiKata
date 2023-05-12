namespace PubApi.Beer.Models;

public class AddABeerRequest
{
    public string? Name { get; set; }
    public decimal PercentageAlcoholByVolume { get; set; }
}
