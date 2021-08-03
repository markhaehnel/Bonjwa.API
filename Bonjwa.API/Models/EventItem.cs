
using Swashbuckle.AspNetCore.Annotations;

namespace Bonjwa.API.Models
{
    public class EventItem
    {
        [SwaggerSchema(Nullable = false)]
        public string Name { get; private set; }

        [SwaggerSchema(Nullable = false)]
        public string Date { get; private set; }

        public EventItem(string name, string date)
        {
            Name = name;
            Date = date;
        }

    }
}
