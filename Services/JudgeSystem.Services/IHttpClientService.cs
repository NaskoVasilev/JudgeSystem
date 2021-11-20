using System.Threading.Tasks;

namespace JudgeSystem.Services
{
    public interface IHttpClientService
    {
        /// <summary>
        /// Post http request to the url.
        /// </summary>
        /// <typeparam name="TResponse">Type to which the response of the request will be deserialized.</typeparam>
        /// <param name="model">Object which will be serialized and sent in the request body.</param>
        /// <param name="url">Request url.</param>
        /// <returns>The response of the request deserialized to object of type TResponse.</returns>
        Task<TResponse> Post<TResponse>(object model, string url);

        /// <summary>
        /// Get http request to the url.
        /// </summary>
        /// <typeparam name="TResponse">Type to which the response of the request will be deserialized.</typeparam>
        /// <param name="url">Request url.</param>
        /// <returns>The response of the request deserialized to object of type TResponse.</returns>
        Task<TResponse> Get<TResponse>(string url);
    }
}
