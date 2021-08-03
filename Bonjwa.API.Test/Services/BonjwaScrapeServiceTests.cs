using System;
using Xunit;
using Bonjwa.API.Services;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Bonjwa.API.Test.Services
{
    public class BonjwaScrapeServiceTest
    {
        [Fact(Skip = "TODO")]
        public async Task TestScrapeAsync()
        {
            var mockLogger = new Mock<ILogger<BonjwaScrapeService>>();
            var service = new BonjwaScrapeService(mockLogger.Object);

            await service.ScrapeEventsAndScheduleAsync();
        }
    }
}
