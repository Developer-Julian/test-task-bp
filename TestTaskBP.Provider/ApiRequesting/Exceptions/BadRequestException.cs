namespace TestTaskBP.Provider.ApiRequesting.Exceptions
{
    public class BadRequestException: ResponseException
    {
        public BadRequestException(string rawResponse)
            : base("Bad request", rawResponse)
        {
        }
    }
}