using System.Globalization;

namespace Bonjwa.API.Config
{
    public static class AppConfig
    {
        /// <summary>Scrape interval in minutes</summary>
        public static readonly int ScrapeInterval = int.Parse(System.Environment.GetEnvironmentVariable("SCRAPE_INTERVAL") ?? "30", CultureInfo.InvariantCulture);

        /// <summary>API-Docs path</summary>
        public static readonly string ApiDocsPath = System.Environment.GetEnvironmentVariable("API_DOCS_PATH") ?? "api-docs";
    }
}
