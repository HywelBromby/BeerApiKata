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

    [HttpGet(Name = "beer")]
    public async Task<IActionResult> Get()
    {
        return await _beerService.GetAllBeers();
    }

    [HttpGet(Name = "beer")]
    public async Task<IActionResult> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPost(Name = "beer")]
    public async Task<IActionResult> Post(AddABeerRequest beer)
    {
        return await _beerService.AddABeer(beer);        
    }

    [HttpPut(Name = "beer")]
    public async Task<IActionResult> Put(UpdateABeerRequest beer)
    {
        throw new NotImplementedException();
    }

}
