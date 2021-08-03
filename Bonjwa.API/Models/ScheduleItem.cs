using System;
using Swashbuckle.AspNetCore.Annotations;

namespace Bonjwa.API.Models
{
    public class ScheduleItem
    {
        [SwaggerSchema(Nullable = false)]
        public string Title { get; private set; }

        [SwaggerSchema(Nullable = false)]
        public string Caster { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public bool Cancelled { get; private set; }

        public ScheduleItem(string title, string caster, DateTime startDate, DateTime endDate, bool cancelled)
        {
            Title = title;
            Caster = caster;
            StartDate = startDate;
            EndDate = endDate;
            Cancelled = cancelled;
        }
    }
}
