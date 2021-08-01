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
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IDataStore _dataStore;

        public ScheduleController(ILogger<ScheduleController> logger, IDataStore dataStore)
        {
            _logger = logger;
            _dataStore = dataStore;
        }

        [HttpGet]
        [SwaggerOperation(Description = "Returns the full schedule")]
        public IEnumerable<ScheduleItem> Get()
        {
            return _dataStore.GetSchedule();
        }
    }
}
