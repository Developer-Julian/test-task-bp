namespace TestTaskBP.Provider.PaymentProcessing.Models
{
    public class CreatePaymentModel
    {
        public string OrderId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public string CardNumber { get; set; }
        public string CardHolder { get; set; }
        public string CardExpiryDate { get; set; }
        public string Cvv { get; set; }
        public string RedirectUrl { get; set; }
    }
}