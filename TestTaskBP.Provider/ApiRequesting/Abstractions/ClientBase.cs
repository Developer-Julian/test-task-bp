using System.Collections.Generic;
using System.Threading.Tasks;
using TestTaskBP.Provider.ApiRequesting.Abstractions.Models;

namespace TestTaskBP.Provider.ApiRequesting.Abstractions
{
    public abstract class ClientBase
    {
        private readonly ClientConfiguration configuration;
        private readonly ISerializer serializer;
        private readonly IHttpClient httpClient;

        protected ClientBase(ClientConfiguration configuration, ISerializer serializer, IHttpClient httpClient)
        {
            this.configuration = configuration;
            this.serializer = serializer;
            this.httpClient = httpClient;
        }

        protected T Deserialize<T>(byte[] serialized) => this.serializer.Deserialize<T>(serialized);
        
        protected byte[] Serialize<T>(T obj) => this.serializer.Serialize(obj);
        
        protected string GetMerchantId() => this.configuration.MerchantId;
        
        protected string GetBaseApi() => this.configuration.ApiBaseUrl;

        protected string GetRequestUrl(RequestDefinition definition) => this.configuration.ApiBaseUrl + definition.Url;

        protected Task<byte[]> ApplyAsync(RequestDefinition definition)
        {
            definition.Headers.Add(new KeyValuePair<string, string>("Content-Type", "application/json"));
            definition.Headers.Add(new KeyValuePair<string, string>("Mechant-Id", this.configuration.MerchantId));
            definition.Headers.Add(new KeyValuePair<string, string>("Secret-Key", this.configuration.SecretKey));
            definition.Url = this.GetRequestUrl(definition);

            return this.httpClient.ApplyAsync(definition);
        }
    }
}