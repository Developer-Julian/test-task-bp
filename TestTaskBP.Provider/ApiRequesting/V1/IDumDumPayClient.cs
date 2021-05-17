using System.Threading.Tasks;
using TestTaskBP.Provider.ApiRequesting.V1.Models;

namespace TestTaskBP.Provider.ApiRequesting.V1
{
    public interface IDumDumPayClient
    {
        /// <summary>
        /// Create new payment
        /// </summary>
        /// <param name="request">Model with data to create payment</param>
        /// <returns>Model with data for further work with payment</returns>
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        
        /// <summary>
        /// Confirm an existing payment
        /// </summary>
        /// <param name="request">Model with data to confirm payment</param>
        /// <returns>Model with confirmed payment data</returns>
        Task<ConfirmPaymentResponse> ConfirmPaymentAsync(ConfirmPaymentRequest request);
        
        /// <summary>
        /// Get the status of an existing payment
        /// </summary>
        /// <param name="request">Model with data to get payment status</param>
        /// <returns>Model with data of an existing payment</returns>
        Task<PaymentStatusResponse> GetPaymentStatusAsync(PaymentStatusRequest request);

        Task<string> PostFormData(ThreeDsRequest request);
    }
}