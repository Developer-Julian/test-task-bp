using System;
using Microsoft.Extensions.DependencyInjection;
using TestTaskBP.Provider.ApiRequesting.Abstractions.Models;
using TestTaskBP.Provider.ApiRequesting.V1;
using TestTaskBP.Provider.PaymentProcessing;
using TestTaskBP.Provider.PaymentProcessing.Models;
using TestTaskBP.Provider.Utility;

namespace TestTaskBP.Provider
{
    public static class DependencyInjectionHelper
    {
        public static IServiceCollection AddDefaultDumDumPay(PaymentConfiguration configuration)
        {
            Guard.ArgumentNotNull(configuration, nameof(configuration));
            
            return new ServiceCollection()
                .AddTransient(serviceProvider => new ClientConfiguration
                {
                    ApiBaseUrl = configuration.ApiBaseUrl,
                    MerchantId = configuration.MerchantId,
                    SecretKey = configuration.SecretKey
                })
                // For your serializer and client implementation. The default is DefaultJsonSerializer and DefaultHttpClient
                // .AddTransient<IHttpClient, DefaultHttpClient>()
                // .AddTransient<ISerializer, DefaultJsonSerializer>()
                .AddTransient<IDumDumPayClient, DumDumPayClient>()
                .AddTransient<IPaymentManager, PaymentManager>();
        }
    }
}