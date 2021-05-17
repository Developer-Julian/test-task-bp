namespace TestTaskBP.Provider.PaymentProcessing.Models
{
    public class PaymentConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string MerchantId { get; set; }
        public string SecretKey { get; set; }
    }
}