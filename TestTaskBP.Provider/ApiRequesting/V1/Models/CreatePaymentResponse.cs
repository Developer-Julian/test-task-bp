namespace TestTaskBP.Provider.ApiRequesting.V1.Models
{
    public class CreatePaymentResponse: ResponseBaseModel<CreatePaymentResultResponse>
    { }
    
    public class CreatePaymentResultResponse
    {
        public string TransactionId { get; set; }
        public string TransactionStatus { get; set; }
        public string PaReq { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
    }
}