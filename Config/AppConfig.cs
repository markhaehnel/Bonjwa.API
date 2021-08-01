namespace Bonjwa.API.Config
{
    public static class AppConfig
    {
        /// <summary>Scrape interval in minutes</summary>
        public static int ScrapeInterval = int.Parse(System.Environment.GetEnvironmentVariable("SCRAPE_INTERVAL") ?? "30");

        /// <summary>API-Docs path</summary>
        public static string ApiDocsPath = System.Environment.GetEnvironmentVariable("API_DOCS_PATH") ?? "api-docs";
    }
}
