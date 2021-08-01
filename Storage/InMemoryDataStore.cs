using System.Collections.Generic;
using System.Linq;
using Bonjwa.API.Models;

namespace Bonjwa.API.Storage
{
    public class InMemoryDataStore : IDataStore
    {
        IEnumerable<EventItem> _eventItems = new List<EventItem>();
        IEnumerable<ScheduleItem> _scheduleItems = new List<ScheduleItem>();

        public IEnumerable<EventItem> GetEvents()
        {
            return _eventItems;
        }
        public void SetEvents(IEnumerable<EventItem> eventItems)
        {
            _eventItems = eventItems;
        }

        public IEnumerable<ScheduleItem> GetSchedule()
        {
            return _scheduleItems;
        }
        public void SetSchedule(IEnumerable<ScheduleItem> scheduleItems)
        {
            _scheduleItems = scheduleItems;
        }
    }
}
