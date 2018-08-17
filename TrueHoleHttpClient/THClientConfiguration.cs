namespace Hypertherm.TrueHoleHttpClient
{
    /// <summary>
    /// Class for passing configuration data.
    /// </summary>
    public class THClientConfiguration : ITHClientConfiguration
    {
        private readonly string _BaseUri;
        private readonly string _SubscriptionHeader;
        private readonly string _SubscriptionKey;

        public string BaseUri
        {
            get => _BaseUri;
        }

        public string SubscriptionHeader
        {
            get => _SubscriptionHeader;
        }

        public string SubscriptionKey
        {
            get => _SubscriptionKey;
        }

        /// <summary>
        /// Constructor, requires a subscription key.  Base URI and subscription header are optional.
        /// </summary>
        /// <param name="subscriptionKey">XPR True Hole API subscription key</param>
        /// <param name="baseUri">Base URL for the XPR True Hole API</param>
        /// <param name="subscriptionHeader">Header name for passing the subscription key</param>
        public THClientConfiguration(string subscriptionKey, 
            string baseUri = null, 
            string subscriptionHeader = null)
        {
            _SubscriptionKey = subscriptionKey;
            
            _BaseUri = baseUri;
            // Use default base URI
            if (string.IsNullOrEmpty(BaseUri))
            {
                _BaseUri = "https://api.hypertherm.com/convert/";
            }

            _SubscriptionHeader = subscriptionHeader;
            // Use default subscription header
            if (string.IsNullOrEmpty(_SubscriptionHeader))
            {
                _SubscriptionHeader = "ocp-apim-subscription-key";
            }
        }
    }
}
