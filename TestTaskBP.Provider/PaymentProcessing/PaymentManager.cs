using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TestTaskBP.Provider.ApiRequesting.V1;
using TestTaskBP.Provider.ApiRequesting.V1.Models;
using TestTaskBP.Provider.Extensions;
using TestTaskBP.Provider.PaymentProcessing.Models;
using TestTaskBP.Provider.Utility;

namespace TestTaskBP.Provider.PaymentProcessing
{
    public class PaymentManager: IPaymentManager
    {
        private const string SuccessPaymentStatus = "Approved";
        private readonly IDumDumPayClient client;
        private readonly ILogger<PaymentManager> logger;
        
        public PaymentManager(IDumDumPayClient client, ILogger<PaymentManager> logger)
        {
            this.client = client;
            this.logger = logger;
        }
        
        public async Task<ConfirmPaymentInfo> CreatePaymentAsync(CreatePaymentModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            
            this.logger.LogTrace($"Start process payment. {model.ToJson()}");
            var createPayment = await this.CreatePaymentInternalAsync(model);
            
            this.logger.LogTrace($"Start 3D Secure: {createPayment.ToJson()}");
            var threeDSecure = await this.Confirm3DSecureInternalAsync(model, createPayment.Result);

            this.logger.LogTrace($"Start confirm payment.");
            var confirmPayment = await this.ConfirmPaymentInternalAsync(createPayment.Result, threeDSecure);

            this.logger.LogTrace($"Check payment status.");
            await this.EnsurePaymentStatusInternalAsync(confirmPayment.Result.TransactionId, confirmPayment.Result.Status);

            this.logger.LogTrace($"The payment was provided successfully.");
            return this.MapToResult(confirmPayment.Result);
        }

        private async Task<CreatePaymentResponse> CreatePaymentInternalAsync(CreatePaymentModel model)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            
            return await this.client.CreatePaymentAsync(new CreatePaymentRequest
            {
                Amount = model.Amount,
                Country = model.Country,
                Currency = model.Currency,
                Cvv = model.Cvv,
                CardHolder = model.CardHolder,
                CardNumber = model.CardNumber,
                OrderId = model.OrderId,
                CardExpiryDate = model.CardExpiryDate
            });
        }

        private async Task<string> Confirm3DSecureInternalAsync(CreatePaymentModel model, CreatePaymentResultResponse createPayment)
        {
            Guard.ArgumentNotNull(model, nameof(model));
            Guard.ArgumentNotNull(createPayment, nameof(createPayment));
            
            return await this.client.PostFormData(new ThreeDsRequest
            {
                PaReq = createPayment.PaReq,
                PostUrl = createPayment.Url
            });
        }

        private async Task<ConfirmPaymentResponse> ConfirmPaymentInternalAsync(CreatePaymentResultResponse createPayment, string paRes)
        {
            Guard.ArgumentNotNull(createPayment, nameof(createPayment));
            Guard.ArgumentNotNullOrEmpty(paRes, nameof(paRes));
            
            return await this.client.ConfirmPaymentAsync(new ConfirmPaymentRequest
            {
                TransactionId = createPayment.TransactionId,
                PaRes = paRes
            });
        }

        private async Task EnsurePaymentStatusInternalAsync(string transactionId, string paymentStatus)
        {
            Guard.ArgumentNotNullOrEmpty(transactionId, nameof(transactionId));
            Guard.ArgumentNotNullOrEmpty(paymentStatus, nameof(paymentStatus));
            
            if (paymentStatus == SuccessPaymentStatus)
            {
                return;
            }
            
            var checkStatus = await this.client.GetPaymentStatusAsync(new PaymentStatusRequest
            {
                TransactionId = transactionId
            });

            if (checkStatus.Result.Status != SuccessPaymentStatus)
            {
                throw new Exception("The processed ");
            }
        }

        private ConfirmPaymentInfo MapToResult(ConfirmPaymentResultResponse response)
        {
            Guard.ArgumentNotNull(response, nameof(response));
            
            return new ConfirmPaymentInfo
            {
                Amount = response.Amount,
                Currency = response.Currency,
                Status = response.Status,
                OrderId = response.OrderId,
                TransactionId = response.TransactionId,
                LastFourDigits = response.LastFourDigits
            };
        }
    }
}