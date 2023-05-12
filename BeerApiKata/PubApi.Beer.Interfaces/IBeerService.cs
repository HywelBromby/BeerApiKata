using PubApi.Domain.Beer.Models;

namespace PubApi.Beer.Interfaces;

public interface IBeerService
{
    Task AddABeer(BeerModel request);   
    Task<IEnumerable<BeerModel>> GetAllBeers();
}
