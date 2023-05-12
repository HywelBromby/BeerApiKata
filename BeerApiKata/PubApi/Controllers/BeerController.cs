using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Interfaces;
using PubApi.Beer.Models;
using System.Net;

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

    [HttpPost(Name = "beer")]
    public async Task<IActionResult> Post(AddABeerRequest beer)
    {        
        return new ObjectResult(await _beerService.AddABeer(beer))
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }


    [HttpGet(Name = "beer")]
    public async Task<IActionResult> Get()
    {
        return new ObjectResult(await _beerService.GetAllBeers())
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
}
