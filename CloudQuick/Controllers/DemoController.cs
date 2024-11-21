using CloudQuick.MyLogging;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace CloudQuick.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {

        private readonly ILogger<DemoController> _logger;

        public DemoController(ILogger<DemoController> logger)
     
        {
            _logger = logger; 
        }

        [HttpGet]
        public ActionResult Index()
        {


            _logger.LogTrace("log message from trace method");
            _logger.LogDebug("log message from Debug method");
            _logger.LogInformation("log message from Information method");
            _logger.LogWarning("log message from Warning method");
            _logger.LogError("log message from Error method");
            _logger.LogCritical("log message from Critical method");

            return Ok();
        }
    }
}
