using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestTaskBP.Provider.ApiRequesting.Abstractions;
using TestTaskBP.Provider.ApiRequesting.Abstractions.Defaults;
using TestTaskBP.Provider.ApiRequesting.Abstractions.Models;
using TestTaskBP.Provider.ApiRequesting.Exceptions;
using TestTaskBP.Provider.ApiRequesting.V1.Models;
using TestTaskBP.Provider.Extensions;
using TestTaskBP.Provider.Utility;

namespace TestTaskBP.Provider.ApiRequesting.V1
{
    public sealed class DumDumPayClient: ClientBase, IDumDumPayClient
    {
        private readonly ILogger<DumDumPayClient> logger;

        public DumDumPayClient(ClientConfiguration configuration, ILogger<DumDumPayClient> logger)
            : base(configuration, new DefaultJsonSerializer(), new DefaultHttpClient(logger))
        {
            this.logger = logger;
        }
        
        public DumDumPayClient(ClientConfiguration configuration, 
            ISerializer serializer, 
            IHttpClient httpClient, 
            ILogger<DumDumPayClient> logger) 
            : base(configuration, serializer, httpClient)
        { 
            this.logger = logger;
            
        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var definition = new RequestDefinition
            {
                Url = "/create",
                Method = HttpMethods.Post,
                Body = this.Serialize(request)
            };

            this.logger.LogTrace($"Start send create payment request to [{definition.Method}] {this.GetRequestUrl(definition)}");
            var rawResult = await this.ApplyAsync(definition);
            return this.Deserialize<CreatePaymentResponse>(rawResult);
        }

        public async Task<ConfirmPaymentResponse> ConfirmPaymentAsync(ConfirmPaymentRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var definition = new RequestDefinition
            {
                Url = "/confirm",
                Method = HttpMethods.Post,
                Body = this.Serialize(request)
            };

            this.logger.LogTrace($"Start send confirm payment request to [{definition.Method}] {this.GetRequestUrl(definition)}");
            var rawResult = await this.ApplyAsync(definition);
            return this.Deserialize<ConfirmPaymentResponse>(rawResult);
        }

        public async Task<PaymentStatusResponse> GetPaymentStatusAsync(PaymentStatusRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            var definition = new RequestDefinition
            {
                Url = $"/{request.TransactionId}/status",
                Method = HttpMethods.Get
            };

            this.logger.LogTrace($"Start send check status payment request to [{definition.Method}] {this.GetRequestUrl(definition)}");
            var rawResult = await this.ApplyAsync(definition);
            return this.Deserialize<PaymentStatusResponse>(rawResult);
        }

        public async Task<string> PostFormData(ThreeDsRequest request)
        {
            Guard.ArgumentNotNull(request, nameof(request));

            request.Md = this.GetMerchantId();
            var data = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(request));
            var webRequest = WebRequest.Create(request.PostUrl);
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = data.Length;
            this.logger.LogTrace($"Start send form data {request.ToJson()}");
            var stream = await webRequest.GetRequestStreamAsync();
            await stream.WriteAsync(data, 0, data.Length);
            stream.Close();
            var response = await webRequest.GetResponseAsync();
            if (response == null)
            {
                throw new BadRequestException($"Failure post form data for 3D secure. Data: {request.ToJson()}. WebRequest: {webRequest.ToJson()}");
            }
            
            string result;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = await sr.ReadToEndAsync();
                sr.Close();
            }

            return result.Split("value").Last().Split("\"")[1];
        }
    }
}