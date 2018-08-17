namespace Hypertherm.TrueHoleHttpClient
{
    /// <summary>
    /// Class for converting a non-True Hole XPR part into a True Hole XPR part.
    /// </summary>
    public class TrueHoleClient
    {
        private ITHApiClient client;

        /// <summary>
        /// Minimal constructor, only requires a subscription key.
        /// </summary>
        /// <param name="subscriptionKey">XPR True Hole API subscription key</param>
        public TrueHoleClient(string subscriptionKey)
            : this(new THClientConfiguration(subscriptionKey)) { }

        /// <summary>
        /// Constructor, requires subscription key and base API URL.
        /// </summary>
        /// <param name="subscriptionKey">XPR True Hole API subscription key</param>
        /// <param name="baseUri">Base URL for the XPR True Hole API</param>
        public TrueHoleClient(string subscriptionKey, string baseUri)
            : this(new THClientConfiguration(subscriptionKey, baseUri)) { }

        /// <summary>
        /// Constructor, requires subscription key, base API URL and subscription header.
        /// </summary>
        /// <param name="subscriptionKey">XPR True Hole API subscription key</param>
        /// <param name="baseUri">Base URL for the XPR True Hole API</param>
        /// <param name="subscriptionHeader">Header name for passing the subscription key</param>
        public TrueHoleClient(string subscriptionKey, string baseUri, string subscriptionHeader) 
            : this(new THClientConfiguration(subscriptionKey, baseUri, subscriptionHeader)) { }

        /// <summary>
        /// Constructor, requires an ITHClientConfiguration object.
        /// </summary>
        /// <param name="thClientConfiguration">An ITHClientConfiguration object</param>
        public TrueHoleClient(ITHClientConfiguration thClientConfiguration) 
            : this(new THApiClient(thClientConfiguration)) { }

        /// <summary>
        /// Constructor, requires an ITHApiClient object.
        /// </summary>
        /// <param name="thApiClient">An ITHApiClient object</param>
        public TrueHoleClient(ITHApiClient thApiClient)
        {
            client = thApiClient;
        }

        /// <summary>
        /// Converts any eligible holes in a single XPR part or nest of parts into True Holes.
        /// </summary>
        /// <param name="settings">Settings file as a string</param>
        /// <param name="part">XPR part file as a string</param>
        /// <returns>A True Hole converted part file.</returns>
        public string Convert(string settings, string part)
        {
            // Upload the settings file
            var settingsFileID = client.Upload(settings);

            // Upload the part file
            var partFileID = client.Upload(part);

            // Convert the uploaded part file
            var convertedFileID = client.Convert(settingsFileID, partFileID);

            // Download the converted part file
            return client.Download(convertedFileID);
        }
    }
}
