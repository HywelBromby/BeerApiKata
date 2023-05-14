using Microsoft.AspNetCore.Mvc;
using PubApi.Bar.Interfaces;
using PubApi.Bar.Models;

namespace PubApi.Controllers;

[ApiController]
[Route("Bar")]
public class BarController : ControllerBase
{
    private readonly IBarService _BarService;

    public BarController(IBarService BarService)
    {
        _BarService = BarService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return await _BarService.GetAllBars();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await _BarService.GetBar(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddABarRequest Bar)
    {
        return await _BarService.AddABar(Bar);        
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateABarRequest Bar)
    {
        return await _BarService.UpdateABar(Bar);
    }

}
