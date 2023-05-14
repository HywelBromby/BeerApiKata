using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Models;

namespace PubApi.Beer.Interfaces;

public interface IBeerService
{
    Task<ObjectResult> AddABeer(AddABeerRequest request);
    Task<ObjectResult> GetBeers(GetBeersRequest filter);
    Task<ObjectResult> UpdateABeer(UpdateABeerRequest request);
    Task<ObjectResult> GetBeer(Guid id);
}
