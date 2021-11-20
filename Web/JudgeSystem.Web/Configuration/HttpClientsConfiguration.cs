using JudgeSystem.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using static JudgeSystem.Common.GlobalConstants;

namespace JudgeSystem.Web.Configuration
{
    public static class HttpClientsConfiguration
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IHttpClientService, HttpClientService>(ConfigureHttpClient);

            return services;
        }

        private static void ConfigureHttpClient(HttpClient client)
          => client.DefaultRequestHeaders.Add(HttpHeaders.Accept, MimeTypes.ApplicationJson);
    }
}
