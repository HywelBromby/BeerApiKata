using BeerApiKata.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PubApi.Beer.Models;
using PubApi.Brewery.Models;
using System.Net;

namespace PubApi.Controllers;

/// <summary>
/// I decided to do this with less abstraction just an all in one file approach
/// </summary>
[ApiController]
public class BreweryBeerController : ControllerBase
{ 
    private readonly IGenericRepository<Guid, BreweryBeerModel> _BreweryBeerRepository;
    private readonly IGenericRepository<Guid, BeerModel> _BeerRepository;
    private readonly IGenericRepository<Guid, BreweryModel> _BreweryRepository;

    public BreweryBeerController(
        IGenericRepository<Guid, BreweryBeerModel> breweryBeerRepository,
        IGenericRepository<Guid, BeerModel> beerRepository,
        IGenericRepository<Guid, BreweryModel> breweryRepository
        )
    {
        _BreweryBeerRepository = breweryBeerRepository;
        _BeerRepository = beerRepository;
        _BreweryRepository = breweryRepository;
    }


    [HttpPost("Brewery/Beer")]
    public async Task<IActionResult> Post(AddABreweryBeerRequest breweryBeer)
    {
        var beer = await _BeerRepository.Get(breweryBeer.BeerId);
        var brewery = await _BreweryRepository.Get(breweryBeer.BreweryId);
        if (brewery == null || beer == null)
        {
            return BadRequest();
        }

        await _BreweryBeerRepository.Create(Guid.NewGuid(), new BreweryBeerModel { BreweryId = breweryBeer.BreweryId, BeerId = breweryBeer.BeerId });
        return new OkResult();
    }

    [HttpGet("Brewery/{id}/Beer")]
    public async Task<IActionResult> Get(Guid id)
    {
        //obvioulsy this is inefficient, and could be filtered in the repository
        var brewery = await _BreweryRepository.Get(id);       
        
        if(brewery == null) { return NotFound(); };

        var breweryBeers = _BreweryBeerRepository.GetAll().Result.Where(i => i.BreweryId == id).Select(i => i.BeerId); 
        var beers = _BeerRepository.GetAll().Result.Where(i => breweryBeers.Contains(i.Id));
        return new ObjectResult(new GetBreweryBeerResponse { Brewery = brewery, Beers = beers })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("Brewery/Beer")]
    public async Task<IActionResult> Get()
    {
        //obvioulsy this is inefficient, and could be filtered in the repository
        var breweries = await _BreweryRepository.GetAll();

        var breweryBeerResponse = new List<GetBreweryBeerResponse>();

        foreach (var brewery in breweries)
        {
            var breweryBeers = _BreweryBeerRepository.GetAll().Result.Where(i => i.BreweryId == brewery.Id).Select(i => i.BeerId); 
            var beers = _BeerRepository.GetAll().Result.Where(i => breweryBeers.Contains(i.Id));
            breweryBeerResponse.Add(new GetBreweryBeerResponse { Brewery = brewery, Beers = beers });
        }
             
        return new ObjectResult(breweryBeerResponse)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
    public class AddABreweryBeerRequest
    {
        public Guid BreweryId { get; set; }
        public Guid BeerId { get; set; }
    }


    public class GetBreweryBeerResponse
    {
        public BreweryModel Brewery { get; set; }
        public IEnumerable<BeerModel> Beers { get; set; }
    }

    public class BreweryBeerModel
    {
        public Guid BreweryId { get; set; }
        public Guid BeerId { get; set; }
    }


}

