using System.Net.Http;

namespace Hypertherm.TrueHoleHttpClient
{
    /// <summary>
    /// Interface for sending HTTP requests.
    /// </summary>
    public interface ITHHttpClient
    {
        /// <summary>
        /// Sends an HTTP Post request with application/json content.
        /// </summary>
        /// <param name="relativeUri">A relative path to an endpoint</param>
        /// <param name="content">Application/json content to send to an endpoint</param>
        /// <param name="client">HTTP client object</param>
        /// <returns>An HttpResponseMessage object</returns>
        HttpResponseMessage PostJson(string relativeUri, string content, HttpClient client);

        /// <summary>
        /// Sends an HTTP Post request with text/plain content.
        /// </summary>
        /// <param name="relativeUri">A relative path to an endpoint</param>
        /// <param name="content">Text/plain content to send to an endpoint</param>
        /// <param name="client">HTTP client object</param>
        /// <returns>An HttpResponseMessage object</returns>
        HttpResponseMessage PostText(string relativeUri, string content, HttpClient client);
    }
}