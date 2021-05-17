using System.Threading.Tasks;
using TestTaskBP.Provider.ApiRequesting.Abstractions.Models;

namespace TestTaskBP.Provider.ApiRequesting.Abstractions
{
    public interface IHttpClient
    {
        Task<byte[]> ApplyAsync(RequestDefinition definition);
    }
}