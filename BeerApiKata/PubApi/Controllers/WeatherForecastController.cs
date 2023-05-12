using Microsoft.AspNetCore.Mvc;

namespace PubApi.Controllers
{
    [ApiController]
    [Route("beer")]
    public class BeerController : ControllerBase
    {
        private readonly ILogger<BeerController> _logger;

        public BeerController(ILogger<BeerController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "beer")]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(new List<string> { "Bud", "Fosters"});
        }
    }
}