using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Models;

namespace PubApi.Beer.Interfaces;

public interface IBeerService
{
    Task<ObjectResult> AddABeer(AddABeerRequest request);   
    Task<ObjectResult> GetAllBeers();
}
