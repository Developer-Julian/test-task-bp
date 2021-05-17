using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TestTaskBP.Provider;
using TestTaskBP.Provider.Extensions;
using TestTaskBP.Provider.PaymentProcessing;
using TestTaskBP.Provider.PaymentProcessing.Models;

namespace TestTaskBP.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = DependencyInjectionHelper.AddDefaultDumDumPay(new PaymentConfiguration
            {
                MerchantId = "6fc3aa31-7afd-4df1-825f-192e60950ca1",
                SecretKey = "53cr3t",
                ApiBaseUrl = "https://private-anon-096c0a47d9-dumdumpay.apiary-mock.com/api/payment"
            });

            serviceCollection
                .AddLogging(configure => configure.AddSerilog());
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .CreateLogger();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var paymentProvider = serviceProvider.GetService<IPaymentManager>();

            try
            {
                var paymentInfo = paymentProvider.CreatePaymentAsync(new CreatePaymentModel
                    {
                        Amount = 123,
                        Country = "CY",
                        Currency = "USD",
                        CardNumber = "4111111111111111",
                        CardHolder = "TEST TESTER",
                        Cvv = "111",
                        CardExpiryDate = "1123",
                        OrderId = "DBB99946-A10A-4D1B-A742-577FA026BC01"
                    })
                    .GetAwaiter()
                    .GetResult();
                Console.WriteLine(paymentInfo.ToJson());
                Console.WriteLine("All done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            Console.ReadKey();
        }
    }
}