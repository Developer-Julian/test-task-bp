namespace TestTaskBP.Provider.ApiRequesting.Abstractions.Models
{
    public sealed class ClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string MerchantId { get; set; }
        public string SecretKey { get; set; }
    }
}