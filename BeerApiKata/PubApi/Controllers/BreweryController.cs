using Microsoft.AspNetCore.Mvc;
using PubApi.Brewery.Interfaces;
using PubApi.Brewery.Models;

namespace PubApi.Controllers;

[ApiController]
[Route("Brewery")]
public class BreweryController : ControllerBase
{
    private readonly IBreweryService _BreweryService;

    public BreweryController(IBreweryService BreweryService)
    {
        _BreweryService = BreweryService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await _BreweryService.GetAllBreweries();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await _BreweryService.GetBrewery(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddABreweryRequest Brewery)
    {
        return await _BreweryService.AddABrewery(Brewery);        
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateABreweryRequest Brewery)
    {
        return await _BreweryService.UpdateABrewery(Brewery);
    }

}
