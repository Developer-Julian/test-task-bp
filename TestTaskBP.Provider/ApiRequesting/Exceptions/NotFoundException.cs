namespace TestTaskBP.Provider.ApiRequesting.Exceptions
{
    public class NotFoundException: ResponseException
    {
        public NotFoundException(string rawResponse)
            : base("Resource not found", rawResponse)
        {
        }
    }
}