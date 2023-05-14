using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Interfaces;
using PubApi.Beer.Models;

namespace PubApi.Controllers;

[ApiController]
[Route("beer")]
public class BeerController : ControllerBase
{
    private readonly IBeerService _beerService;

    public BeerController(IBeerService beerService)
    {
        _beerService = beerService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await _beerService.GetAllBeers();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await _beerService.GetBeer(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddABeerRequest beer)
    {
        return await _beerService.AddABeer(beer);        
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateABeerRequest beer)
    {
        return await _beerService.UpdateABeer(beer);
    }

}
