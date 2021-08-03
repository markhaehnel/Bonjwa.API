using System.Collections.Generic;
using Bonjwa.API.Models;

namespace Bonjwa.API.Storage
{
    public interface IDataStore
    {
        IEnumerable<EventItem> GetEvents();
        void SetEvents(IEnumerable<EventItem> scheduleItems);
        IEnumerable<ScheduleItem> GetSchedule();
        void SetSchedule(IEnumerable<ScheduleItem> scheduleItems);
    }
}
