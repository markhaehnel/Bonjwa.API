using System;
using System.Threading.Tasks;

namespace Bonjwa.API.Services
{
    public interface IFetchService
    {
        Task<string> FetchAsync(Uri uri);
    }
}
