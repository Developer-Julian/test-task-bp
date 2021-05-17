using System;

namespace TestTaskBP.Provider.ApiRequesting.Exceptions
{
    public class ResponseException : Exception
    {
        private string rawResponse;
        public ResponseException(string message, string rawResponse, Exception innerException = null)
            : base(message, innerException)
        {
            this.RawResponse = rawResponse;
        }

        public string RawResponse
        {
            get => this.rawResponse;
            private set
            {
                this.rawResponse = value;
                this.Data.Add("RawMessage", value);
            }
        }
    }
}