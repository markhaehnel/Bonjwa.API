using System;
using Xunit;
using Bonjwa.API.Services;
using Bonjwa.API.Tests.TestUtils;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace Bonjwa.API.Tests.Services
{
    public class BonjwaScrapeServiceTest
    {
        [Theory]
        [PlainFileDataAttribute("TestData/sample1.html")]
        public async Task TestScrapeAsync(string html)
        {
            var mockLogger = new Mock<ILogger<BonjwaScrapeService>>();
            var mockFetcher = new Mock<IFetchService>();
            mockFetcher.Setup(x => x.FetchAsync(It.IsAny<Uri>()).Result).Returns(html);

            var service = new BonjwaScrapeService(mockLogger.Object, mockFetcher.Object);

            var (eventItems, scheduleItems) = await service.ScrapeEventsAndScheduleAsync().ConfigureAwait(false);

            Assert.Equal(3, eventItems.Count);
            Assert.Equal("Bonjwa Achievement Show powered by yello #Werbung", eventItems.First().Name);
            Assert.Equal("1. August", eventItems.First().Date);
            Assert.Equal("Chätzen", eventItems.Last().Name);
            Assert.Equal("Mitte August", eventItems.Last().Date);

            Assert.Equal(24, scheduleItems.Count);

            var firstItem = scheduleItems.First();
            Assert.Single(scheduleItems.Where(x => x.Cancelled));
            Assert.Equal("The Elder Scrolls Online", firstItem.Title);
            Assert.Equal("Niklas", firstItem.Caster);
            Assert.Equal(DateTime.Parse("2021-07-26T16:00:00.0000000+02:00", CultureInfo.InvariantCulture), firstItem.StartDate);
            Assert.Equal(DateTime.Parse("2021-07-26T18:00:00.0000000+02:00", CultureInfo.InvariantCulture), firstItem.EndDate);
            Assert.False(firstItem.Cancelled);

            var lastItem = scheduleItems.Last();
            Assert.Equal("PUBG", lastItem.Title);
            Assert.Equal("Leon & Matteo & Marius &", lastItem.Caster);
            Assert.Equal(DateTime.Parse("2021-07-29T00:00:00.0000000+02:00", CultureInfo.InvariantCulture), lastItem.StartDate);
            Assert.Equal(DateTime.Parse("2021-07-29T03:00:00.0000000+02:00", CultureInfo.InvariantCulture), lastItem.EndDate);
            Assert.False(firstItem.Cancelled);

            var cancelledItem = scheduleItems.ElementAt(5);
            Assert.True(cancelledItem.Cancelled);

            var emptyCasterItem = scheduleItems.ElementAt(1);
            Assert.Empty(emptyCasterItem.Caster);
        }
    }
}
