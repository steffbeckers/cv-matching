using Microsoft.AspNetCore.Mvc;

namespace RJM.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet]
        public IActionResult Healthcheck()
        {
            return Ok();
        }
    }
}
