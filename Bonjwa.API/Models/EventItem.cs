
using Swashbuckle.AspNetCore.Annotations;

namespace Bonjwa.API.Models
{
    public class EventItem
    {
        [SwaggerSchema(Nullable = false)]
        public string Title { get; private set; }

        [SwaggerSchema(Nullable = false)]
        public string Date { get; private set; }

        public EventItem(string title, string date)
        {
            Title = title;
            Date = date;
        }

    }
}
