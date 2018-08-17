using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Hypertherm.TrueHoleHttpClient
{
    /// <summary>
    /// Exception for XPR True Hole client errors.
    /// </summary>
    public class THApiClientException : Exception
    {
        /// <summary>
        /// Constructor, requires a message string.
        /// </summary>
        /// <param name="message">An error message string</param>
        public THApiClientException(string message) : base(message) { }
    }

    /// <summary>
    /// Class for calling individual endpoints on the XPR True Hole API.
    /// </summary>
    public class THApiClient : ITHApiClient
    {

        public const string TextPlain = "text/plain";
        public const string ApplicationJson = "application/json";

        private HttpClient client;
        ITHHttpClient thHttpClient;

        /// <summary>
        /// Constructor, requires an ITHClientConfiguration object.
        /// </summary>
        /// <param name="thClientConfiguration">An ITHClientConfiguration object</param>
        public THApiClient(ITHClientConfiguration thClientConfiguration) 
            : this(new THHttpClient(), thClientConfiguration){}

        /// <summary>
        /// Constructor, requires an ITHHttpClient object and an ITHClientConfiguration object.
        /// </summary>
        /// <param name="thHttpClient">An ITHHttpClient object</param>
        /// <param name="thClientConfiguration">An ITHClientConfiguration object</param>
        public THApiClient(ITHHttpClient thHttpClient, ITHClientConfiguration thClientConfiguration)
        {
            this.thHttpClient = thHttpClient;
            client = new HttpClient();

            // Force the HTTP client to use TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Set base URL
            client.BaseAddress = new Uri(thClientConfiguration.BaseUri);
            // Set subscription key header
            client.DefaultRequestHeaders.Add(thClientConfiguration.SubscriptionHeader, 
                thClientConfiguration.SubscriptionKey);                        
        }

        /// <summary>
        /// Uploads a settings or part string to the XPR True Hole API and returns a file ID.
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>A file ID</returns>
        public string Upload(string content)
        {
            string fileID = null;

            // Set Accepts header to application/json
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));

            // Send upload HTTP request
            HttpResponseMessage response = thHttpClient.PostText("api/upload", content, client);

            // Handle json response
            if (response.Content.Headers?.ContentType?.MediaType == ApplicationJson)
            {
                JObject jsonResponse = HandleJsonResponse(response);

                fileID = jsonResponse.Property("fileID")?.Value?.ToString()?.Trim();
            }
            else
            {
                throw new THApiClientException($"Response did not contain Content-Type: {ApplicationJson}.");
            }

            return fileID;
        }

        /// <summary>
        /// Converts the uploaded part file into a True Hole file by passing in the Settings File ID and Part File ID.
        /// </summary>
        /// <param name="settingsFileID">A settings file ID</param>
        /// <param name="partFileID">A part file ID</param>
        /// <returns>A True Hole file ID</returns>
        public string Convert(string settingsFileID, string partFileID)
        {
            string convertedPartFileID = null;

            // Set Accepts header to application/json
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationJson));

            // Create json to send to API
            var content = JsonConvert.SerializeObject(new
            {
                settingsFileID,
                partFileID
            });

            // Send convert HTTP request
            HttpResponseMessage response = thHttpClient.PostJson("api/convert", content, client);

            // Handle json response
            if (response.Content.Headers?.ContentType?.MediaType == ApplicationJson)
            {
                JObject jsonResponse = HandleJsonResponse(response);

                convertedPartFileID = jsonResponse.Property("convertedPartFileID")?.Value?.ToString()?.Trim();
            }
            else
            {
                throw new THApiClientException($"Response did not contain Content-Type: {ApplicationJson}.");
            }

            return convertedPartFileID;
        }

        /// <summary>
        /// Downloads the True Hole part program by passing the True Hole File ID.
        /// </summary>
        /// <param name="fileID">A True Hole file ID</param>
        /// <returns>A converted part string</returns>
        public string Download(string fileID)
        {
            string errorMessage = null;

            // Set Accepts header to text/plain
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(TextPlain));

            // Create json to send to API
            var content = JsonConvert.SerializeObject(new
            {
                fileID
            });

            // Send download HTTP request
            HttpResponseMessage response = thHttpClient.PostJson("api/download", content, client);

            var contentType = response.Content.Headers?.ContentType?.MediaType;

            // Handle text/plain response
            if (contentType == TextPlain)
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                errorMessage = response.Content.ReadAsStringAsync().Result;
            }
            // Read any proxy errors from json
            else if (contentType == ApplicationJson && !response.IsSuccessStatusCode)
            {
                HandleJsonResponse(response);
            }
            else
            {
                errorMessage = $"Response did not contain Content-Type: {TextPlain}.";
            }

            throw new THApiClientException(errorMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response">An HttpResponseMessage object</param>
        /// <returns>A JObject representation of the json response</returns>
        private JObject HandleJsonResponse(HttpResponseMessage response)
        {
            string errorMessage = null;

            try
            {
                JObject jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                if (response.IsSuccessStatusCode)
                {
                    return jsonResponse;
                }

                // Read Hypertherm error message
                errorMessage = jsonResponse.Property("errorMsg")?.Value?.ToString().Trim();

                // Read proxy error message
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = jsonResponse.Property("message")?.Value?.ToString().Trim();
                }

                // Report an unknown error
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "Response contained an unknown error.";
                }
            }
            catch (JsonReaderException)
            {
                errorMessage = "Response contained malformed json.";                                
            }

            throw new THApiClientException(errorMessage);
        }
    }
}
