using System.Collections.Generic;

namespace TestTaskBP.Provider.ApiRequesting.Abstractions.Models
{
    public sealed class RequestDefinition
    {
        public string Url { get; set; } = string.Empty;
        public string Method { get; set; } = HttpMethods.Get;
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public byte[] Body { get; set; } = null;
    }
}