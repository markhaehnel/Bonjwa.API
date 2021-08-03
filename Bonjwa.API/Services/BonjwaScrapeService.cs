using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using Bonjwa.API.Models;
using Microsoft.Extensions.Logging;

namespace Bonjwa.API.Services
{
    public class BonjwaScrapeService
    {
        private readonly ILogger<BonjwaScrapeService> _logger;

        public BonjwaScrapeService(ILogger<BonjwaScrapeService> logger)
        {
            _logger = logger;
        }

        public async Task<(List<EventItem>, List<ScheduleItem>)> ScrapeEventsAndScheduleAsync()
        {
            var document = await fetchDocument();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var angleDoc = await context.OpenAsync(req => req.Content(document));

            var eventItems = this.extractEvents(angleDoc);
            var scheduleItems = this.extractSchedule(angleDoc);

            return (eventItems, scheduleItems);
        }

        async private Task<string> fetchDocument()
        {
            var result = string.Empty;
            using (var webClient = new System.Net.WebClient())
            {
                result = await webClient.DownloadStringTaskAsync(new Uri("https://bonjwa.de/programm"));
            }

            return result;
        }

        private List<EventItem> extractEvents(AngleSharp.Dom.IDocument document)
        {
            var elements = document.QuerySelectorAll(".c-content-three table tr");

            var eventItems = new List<EventItem>();

            foreach (var element in elements)
            {
                var content = element.QuerySelectorAll("td");
                if (
                    content.Length != 2
                    || content[0].TextContent.Trim() == string.Empty
                    || content[1].TextContent.Trim() == string.Empty
                ) continue;

                var title = content[0].TextContent.Trim();
                var date = content[1].TextContent.Trim();

                eventItems.Add(new EventItem(date, title));
            }

            return eventItems;
        }

        private List<ScheduleItem> extractSchedule(AngleSharp.Dom.IDocument document)
        {
            var elements = document.QuerySelectorAll(".stream-plan>table>tbody>tr>td");

            var scheduleItems = new List<ScheduleItem>();

            foreach (var element in elements)
            {
                if (element.Attributes.GetNamedItem("free-streaming-slot") != null) continue;

                var content = element.QuerySelectorAll("p");

                if (content.Length < 1) continue;

                var title = content.ElementAtOrDefault(0).TextContent.Trim();
                var caster = content.ElementAtOrDefault(1)?.TextContent.Trim() ?? "";
                var date = element.Attributes.GetNamedItem("data-date")?.Value;
                var hourStart = element.Attributes.GetNamedItem("data-hour-start")?.Value;
                var hourEnd = element.Attributes.GetNamedItem("data-hour-end")?.Value;
                var cancelled = element.ClassList.Contains("cancelled-streaming-slot");

                var dateParts = date.Split('-');
                var year = dateParts[0];
                var month = dateParts[1].Length == 2 ? dateParts[1] : dateParts[1].Insert(0, "0");
                var day = dateParts[2].Length == 2 ? dateParts[2] : dateParts[2].Insert(0, "0");
                hourStart = hourStart.Length == 2 ? hourStart : hourStart.Insert(0, "0");
                hourEnd = hourEnd.Length == 2 ? hourEnd : hourEnd.Insert(0, "0");

                var startOverlap = false;
                var endOverlap = false;

                if (hourStart == "24")
                {
                    startOverlap = true;
                    hourStart = "00";
                }

                if (hourEnd == "24")
                {
                    endOverlap = true;
                    hourEnd = "00";
                }


                var tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
                var offsetHours = tz.GetUtcOffset(DateTime.UtcNow).Hours;

                var startDate = DateTime.Parse($"{year}-{month}-{day}T{hourStart}:00:00.000+{offsetHours}:00");
                var endDate = DateTime.Parse($"{year}-{month}-{day}T{hourEnd}:00:00.000+{offsetHours}:00");

                startDate = startOverlap ? startDate.AddDays(1) : startDate;
                endDate = endOverlap ? endDate.AddDays(1) : endDate;

                scheduleItems.Add(new ScheduleItem(title, caster, startDate, endDate, cancelled));
            }

            return scheduleItems;
        }

    }
}
