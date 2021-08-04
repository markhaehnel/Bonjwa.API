using Xunit;
using Bonjwa.API.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using Bonjwa.API.Storage;
using System.Collections.Generic;
using Bonjwa.API.Models;
using System.Linq;
using System;

namespace Bonjwa.API.Tests.Controllers
{
    public class ScheduleControllerTests
    {
        private readonly List<ScheduleItem> fakeScheduleData = new List<ScheduleItem> {
            new ScheduleItem("Test Schedule Item 1", "Test Caster 1", DateTime.Now, DateTime.Now, false),
            new ScheduleItem("Test Schedule Item 2", "Test Caster 2", DateTime.Now, DateTime.Now, true),
            new ScheduleItem("Test Schedule Item 3", "Test Caster 3", DateTime.Now, DateTime.Now, false)
        };
        private readonly List<ScheduleItem> fakeEmptyScheduleData = new List<ScheduleItem>();

        [Fact]
        public void GetReturnsEventItemsFromStore()
        {
            var mockLogger = new Mock<ILogger<ScheduleController>>();
            var mockStore = new Mock<IDataStore>();
            mockStore.Setup(x => x.GetSchedule()).Returns(fakeScheduleData);

            var service = new ScheduleController(mockLogger.Object, mockStore.Object);

            Assert.True(service.Get().Count() == 3);
            Assert.Equal(service.Get(), fakeScheduleData);
        }

        [Fact]
        public void GetReturnsEmptyScheduleItemsArrayWhenStoryIsEmpty()
        {
            var mockLogger = new Mock<ILogger<ScheduleController>>();
            var mockStore = new Mock<IDataStore>();
            mockStore.Setup(x => x.GetSchedule()).Returns(fakeEmptyScheduleData);

            var service = new ScheduleController(mockLogger.Object, mockStore.Object);

            Assert.True(service.Get().Count() == 0);
        }
    }
}
