using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Interfaces;
using PubApi.Domain.Beer.Models;
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
    public async Task<IActionResult> Post(BeerModel beer)
    {
        try
        {
            await _beerService.AddABeer(beer);
            return new OkResult();
        }
        catch (Exception ex)
        {
            //obvioulsy we may want to not return the message for security reasons, rather we could return a strucutred response
            return new ObjectResult(ex.Message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }


    [HttpGet(Name = "beer")]
    public async Task<IActionResult> Get()
    {
        try
        {
            return new ObjectResult(await _beerService.GetAllBeers())
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        catch (Exception ex)
        {
            //obvioulsy we may want to not return the message for security reasons, rather we could return a strucutred response
            return new ObjectResult(ex.Message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
