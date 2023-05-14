using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Models;

namespace PubApi.Beer.Interfaces;

public interface IBeerService
{
    Task<ObjectResult> AddABeer(AddABeerRequest request);   
    Task<ObjectResult> GetAllBeers();
    Task<ObjectResult> UpdateABeer(UpdateABeerRequest request);
    Task<ObjectResult> GetBeer(Guid id);
}
