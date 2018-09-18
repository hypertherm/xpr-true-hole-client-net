using System.Net.Http;
using System.Text;

namespace Hypertherm.XprTrueHoleHttpClient
{
    /// <summary>
    /// Class for sending HTTP requests.
    /// </summary>
    public class THHttpClient : ITHHttpClient
    {
        /// <summary>
        /// Sends an HTTP Post request with text/plain content.
        /// </summary>
        /// <param name="relativeUri">A relative path to an endpoint</param>
        /// <param name="content">Text/plain content to send to an endpoint</param>
        /// <param name="client">HTTP client object</param>
        /// <returns>An HttpResponseMessage object</returns>
        public HttpResponseMessage PostText(string relativeUri, string content, HttpClient client)
        {
            return client.PostAsync(relativeUri, new StringContent(content)).Result;
        }

        /// <summary>
        /// Sends an HTTP Post request with application/json content.
        /// </summary>
        /// <param name="relativeUri">A relative path to an endpoint</param>
        /// <param name="content">Application/json content to send to an endpoint</param>
        /// <param name="client">HTTP client object</param>
        /// <returns>An HttpResponseMessage object</returns>
        public HttpResponseMessage PostJson(string relativeUri, string content, HttpClient client)
        {
            return client.PostAsync(relativeUri, new StringContent(content, Encoding.UTF8, THApiClient.ApplicationJson)).Result;
        }
    }
}
