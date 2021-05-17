using System.Threading.Tasks;
using TestTaskBP.Provider.PaymentProcessing.Models;

namespace TestTaskBP.Provider.PaymentProcessing
{
    public interface IPaymentManager
    {
        Task<ConfirmPaymentInfo> CreatePaymentAsync(CreatePaymentModel model);
    }
}