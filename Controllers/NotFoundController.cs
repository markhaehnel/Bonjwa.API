using Bonjwa.API.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bonjwa.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/404")]
    public class NotFoundController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;

        public NotFoundController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            return new
            {
                status = 404,
                message = "Not Found",
                documentationUrl = $"/{AppConfig.ApiDocsPath}"
            };
        }
    }
}
