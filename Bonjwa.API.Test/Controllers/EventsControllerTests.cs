using Xunit;
using Bonjwa.API.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using Bonjwa.API.Storage;
using System.Collections.Generic;
using Bonjwa.API.Models;
using System.Linq;

namespace Bonjwa.API.Test.Controllers
{
    public class EventsControllerTests
    {
        private readonly List<EventItem> fakeEventData = new List<EventItem> {
            new EventItem("Test Event 1", "1. August"),
            new EventItem("Test Event 2", "2. August"),
            new EventItem("Test Event 3", "3. August"),
        };
        private readonly List<EventItem> fakeEmptyEventData = new List<EventItem>();

        [Fact]
        public void GetReturnsEmptyEventItemsArrayWhenStoryIsEmpty()
        {
            var mockLogger = new Mock<ILogger<EventsController>>();
            var mockStore = new Mock<IDataStore>();
            mockStore.Setup(x => x.GetEvents()).Returns(fakeEventData);

            var service = new EventsController(mockLogger.Object, mockStore.Object);

            Assert.True(service.Get().Count() == 3);
            Assert.Equal(service.Get(), fakeEventData);
        }

        [Fact]
        public void GetReturnsNoEventWhenStoryIsEmpty()
        {
            var mockLogger = new Mock<ILogger<EventsController>>();
            var mockStore = new Mock<IDataStore>();
            mockStore.Setup(x => x.GetEvents()).Returns(fakeEmptyEventData);

            var service = new EventsController(mockLogger.Object, mockStore.Object);

            Assert.True(service.Get().Count() == 0);
        }
    }
}
