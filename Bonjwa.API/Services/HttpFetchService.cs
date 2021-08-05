using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bonjwa.API.Services
{
    public class HttpFetchService : IFetchService, IDisposable
    {
        private readonly HttpClient _client = new HttpClient();

        public Task<string> FetchAsync(Uri uri)
        {
            return _client.GetStringAsync(uri);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _client?.Dispose();

            }
        }
    }
}
