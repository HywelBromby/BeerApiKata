using PubApi.Beer.Models;

namespace PubApi.Beer.Interfaces;

public interface IBeerService
{
    Task<Guid> AddABeer(AddABeerRequest request);   
    Task<IEnumerable<BeerModel>> GetAllBeers();
}
