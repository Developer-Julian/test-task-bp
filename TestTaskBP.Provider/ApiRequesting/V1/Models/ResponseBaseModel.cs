namespace TestTaskBP.Provider.ApiRequesting.V1.Models
{
    public class ResponseBaseModel<T>
    {
        public T Result { get; set; }
        public ErrorResponse Errors { get; set; }
    }
}