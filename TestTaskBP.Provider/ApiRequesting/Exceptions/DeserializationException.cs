using System;

namespace TestTaskBP.Provider.ApiRequesting.Exceptions
{
    public class DeserializationException : ResponseException
    {
        public DeserializationException(string rawResponse, Exception innerException)
            : base("Deserialization error", rawResponse, innerException)
        {
        }
    }
}