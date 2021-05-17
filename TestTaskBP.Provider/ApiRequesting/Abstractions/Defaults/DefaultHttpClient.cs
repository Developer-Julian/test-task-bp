using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestTaskBP.Provider.ApiRequesting.Abstractions.Models;
using TestTaskBP.Provider.ApiRequesting.Exceptions;
using TestTaskBP.Provider.ApiRequesting.V1;
using TestTaskBP.Provider.ApiRequesting.V1.Models;

namespace TestTaskBP.Provider.ApiRequesting.Abstractions.Defaults
{
    internal sealed class DefaultHttpClient: IHttpClient, IDisposable
    {
        private readonly HttpClient client = new HttpClient();
        
        private readonly ILogger<DumDumPayClient> logger;

        public DefaultHttpClient(ILogger<DumDumPayClient> logger)
        {
            this.logger = logger;
        }
        
        public  async Task<byte[]> ApplyAsync(RequestDefinition definition)
        {
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(definition.Url),
                Method = new HttpMethod(definition.Method),
            };
            foreach (var (key, value) in definition.Headers)
            {
                message.Headers.TryAddWithoutValidation(key, value);
            }

            if (definition.Body != null)
            {
                message.Content = new ByteArrayContent(definition.Body);
                foreach (var (key, value) in definition.Headers)
                {
                    message.Content.Headers.TryAddWithoutValidation(key, value);
                }
            }
            
            var stopwatch = Stopwatch.StartNew();
            var response = await this.client.SendAsync(message);
            stopwatch.Stop();
            var result = await response.Content.ReadAsByteArrayAsync();

            if (response.IsSuccessStatusCode)
            {
                this.logger.LogTrace($"Request: {definition.Method} {definition.Url}. Elapsed: {stopwatch.ElapsedMilliseconds}. ResponseCode: {response.StatusCode}.");
                return result;
            }

            var strResult = Encoding.UTF8.GetString(result);
            this.logger.LogError($"Failure http status code: {response.StatusCode}. Message: {strResult}");
            var errorResponse =
                JsonConvert.DeserializeObject<ResponseBaseModel<ResponseWithoutResultsModel>>(strResult);
            if (errorResponse != null)
            {
                strResult = $"{errorResponse.Errors.Type}: {errorResponse.Errors.Message}";
            }

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(strResult);
                case HttpStatusCode.NotFound:
                    throw new NotFoundException(strResult);
                default:
                    throw new ResponseException($"Api return not success code: {response.StatusCode}", strResult);
            };
        }
        
        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}