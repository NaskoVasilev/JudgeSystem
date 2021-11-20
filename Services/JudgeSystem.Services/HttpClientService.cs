using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using JudgeSystem.Common.Extensions;
using static JudgeSystem.Common.GlobalConstants;

namespace JudgeSystem.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient client;

        public HttpClientService(HttpClient client)
        {
            this.client = client;
            this.client.DefaultRequestHeaders.Add(HttpHeaders.Accept, MimeTypes.ApplicationJson);
        }

        public async Task<TResponse> Post<TResponse>(object model, string url)
        {
            if (model == null)
            {
                throw new ArgumentException("The model cannot be null!");
            }

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("The url cannot be null!");
            }

            string body = model.ToJson();
            var requestContent = new StringContent(body, Encoding.UTF8, MimeTypes.ApplicationJson);

            return await Post<TResponse>(requestContent, url);
        }

        public async Task<TResponse> Get<TResponse>(string url)
        {
            HttpResponseMessage responseMessage = await GetResponse(url);
            string content = await responseMessage.Content.ReadAsStringAsync();
            return content.FromJson<TResponse>();
        }

        private async Task<HttpResponseMessage> GetResponse(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("The url cannot be null!");
            }

            HttpResponseMessage responseMessage = await client.GetAsync(url);
            await ValidateResponseMessage(responseMessage);

            return responseMessage;
        }

        private async Task<TResponse> Post<TResponse>(HttpContent content, string url)
        {
            HttpResponseMessage responseMessage = await client.PostAsync(url, content);
            await ValidateResponseMessage(responseMessage);
            string result = await responseMessage.Content.ReadAsStringAsync();

            return result.FromJson<TResponse>();
        }

        private async Task ValidateResponseMessage(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                string errorMessage = $"{responseMessage.RequestMessage.Method.Method} request to {responseMessage.RequestMessage.RequestUri} failed. " +
                    $"Status code: {responseMessage.StatusCode}.";
                string content = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    errorMessage += Environment.NewLine + content;
                }

                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
