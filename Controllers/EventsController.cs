using System.Collections.Generic;
using Bonjwa.API.Models;
using Bonjwa.API.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Bonjwa.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IDataStore _dataStore;

        public EventsController(ILogger<EventsController> logger, IDataStore dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        [HttpGet]
        [SwaggerOperation(Description = "Returns all upcoming events")]
        public IEnumerable<EventItem> Get()
        {
            return _dataStore.GetEvents();
        }
    }
}
