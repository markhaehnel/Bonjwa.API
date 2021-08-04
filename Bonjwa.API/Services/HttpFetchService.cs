using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bonjwa.API.Services
{
    public class HttpFetchService : IFetchService, IDisposable
    {
        private readonly HttpClient _client = new HttpClient();

        public Task<string> FetchAsync(string url)
        {
            var uri = new Uri(url);
            return _client.GetStringAsync(uri);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
