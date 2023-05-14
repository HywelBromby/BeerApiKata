using BeerApiKata.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PubApi.Bar.Models;
using PubApi.Beer.Models;
using System.Net;

namespace PubApi.Controllers;

/// <summary>
/// I decided to do this with less abstraction just an all in one file approach
/// </summary>
[ApiController]
public class BarBeerController : ControllerBase
{ 
    private readonly IGenericRepository<Guid, BarBeerModel> _BarBeerRepository;
    private readonly IGenericRepository<Guid, BeerModel> _BeerRepository;
    private readonly IGenericRepository<Guid, BarModel> _BarRepository;

    public BarBeerController(
        IGenericRepository<Guid, BarBeerModel> BarBeerRepository,
        IGenericRepository<Guid, BeerModel> beerRepository,
        IGenericRepository<Guid, BarModel> barRepository
        )
    {
        _BarBeerRepository = BarBeerRepository;
        _BeerRepository = beerRepository;
        _BarRepository = barRepository;
    }


    [HttpPost("Bar/Beer")]
    public async Task<IActionResult> Post(AddABarBeerRequest barBeer)
    {       
        var beer = await _BeerRepository.Get(barBeer.BeerId);
        var bar = await _BarRepository.Get(barBeer.BarId);
        if(bar == null || beer == null) 
        {
            return BadRequest(); 
        }
        
        await _BarBeerRepository.Create(Guid.NewGuid(), new BarBeerModel { BarId = barBeer.BarId, BeerId = barBeer.BeerId });
        return new OkResult();
    }

    [HttpGet("Bar/{id}/Beer")]
    public async Task<IActionResult> Get(Guid id)
    {
        //obvioulsy this is inefficient, and could be filtered in the repository
        var bar = await _BarRepository.Get(id);       
        
        if(bar == null) { return NotFound(); };

        var BarBeers = _BarBeerRepository.GetAll().Result.Where(i => i.BarId == id).Select(i => i.BeerId); 
        var beers = _BeerRepository.GetAll().Result.Where(i => BarBeers.Contains(i.Id));
        return new ObjectResult(new GetBarBeerResponse { Bar = bar, Beers = beers })
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }

    [HttpGet("Bar/Beer")]
    public async Task<IActionResult> Get()
    {
        //obvioulsy this is inefficient, and could be filtered in the repository
        var bars = await _BarRepository.GetAll();

        var BarBeerResponse = new List<GetBarBeerResponse>();

        foreach (var bar in bars)
        {
            var BarBeers = _BarBeerRepository.GetAll().Result.Where(i => i.BarId == bar.Id).Select(i => i.BeerId); 
            var beers = _BeerRepository.GetAll().Result.Where(i => BarBeers.Contains(i.Id));
            BarBeerResponse.Add(new GetBarBeerResponse { Bar = bar, Beers = beers });
        }
             
        return new ObjectResult(BarBeerResponse)
        {
            StatusCode = (int)HttpStatusCode.OK
        };
    }
    public class AddABarBeerRequest
    {
        public Guid BarId { get; set; }
        public Guid BeerId { get; set; }
    }


    public class GetBarBeerResponse
    {
        public BarModel Bar { get; set; }
        public IEnumerable<BeerModel> Beers { get; set; }
    }

    public class BarBeerModel
    {
        public Guid BarId { get; set; }
        public Guid BeerId { get; set; }
    }
}

