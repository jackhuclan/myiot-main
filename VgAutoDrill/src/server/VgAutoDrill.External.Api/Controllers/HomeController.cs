using Microsoft.AspNetCore.Mvc;

namespace VgAutoDrill.External.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        ///Health Check
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckHealth")]
        public async Task<ActionResult> CheckHealth()
        {
            return Ok("hello world！");
        }
    }
}
