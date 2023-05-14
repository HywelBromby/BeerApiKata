using Microsoft.AspNetCore.Mvc;
using PubApi.Brewery.Models;

namespace PubApi.Brewery.Interfaces;

public interface IBreweryService
{
    Task<ObjectResult> AddABrewery(AddABreweryRequest request);
    Task<ObjectResult> GetAllBreweries();
    Task<ObjectResult> UpdateABrewery(UpdateABreweryRequest request);
    Task<ObjectResult> GetBrewery(Guid id);
}
