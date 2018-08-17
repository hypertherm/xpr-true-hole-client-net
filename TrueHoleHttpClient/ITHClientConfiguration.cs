namespace Hypertherm.TrueHoleHttpClient
{
    /// <summary>
    /// Interface for passing configuration data.
    /// </summary>
    public interface ITHClientConfiguration
    {
        /// <summary>
        /// A Base URL for the XPR True Hole API.
        /// </summary>
        string BaseUri { get; }

        /// <summary>
        /// A header name for passing the subscription key.
        /// </summary>
        string SubscriptionHeader { get; }

        /// <summary>
        /// An XPR True Hole API subscription key.
        /// </summary>
        string SubscriptionKey { get; }
    }
}