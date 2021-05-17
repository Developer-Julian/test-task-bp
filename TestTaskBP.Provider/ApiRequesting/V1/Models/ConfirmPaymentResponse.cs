namespace TestTaskBP.Provider.ApiRequesting.V1.Models
{
    public class ConfirmPaymentResponse: ResponseBaseModel<ConfirmPaymentResultResponse>
    {  }
    
    public class ConfirmPaymentResultResponse
    {
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public string LastFourDigits { get; set; }
    }
}