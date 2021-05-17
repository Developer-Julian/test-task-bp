using System;
using System.Text;
using Newtonsoft.Json;
using TestTaskBP.Provider.ApiRequesting.Exceptions;

namespace TestTaskBP.Provider.ApiRequesting.Abstractions.Defaults
{
    internal sealed class DefaultJsonSerializer : ISerializer
    {
        public T Deserialize<T>(byte[] serialized)
        {
            var str = Encoding.UTF8.GetString(serialized);
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception ex)
            {
                throw new DeserializationException(str, ex);
            }
        }

        public byte[] Serialize<T>(T obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(str);
        }
    }
}