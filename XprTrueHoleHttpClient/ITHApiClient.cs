namespace Hypertherm.XprTrueHoleHttpClient
{
    /// <summary>
    /// Interface for calling individual endpoints on the XPR True Hole API.
    /// </summary>
    public interface ITHApiClient
    {
        /// <summary>
        /// Uploads a settings or part string to the XPR True Hole API and returns a file ID.
        /// </summary>
        /// <param name="content">String content</param>
        /// <returns>A file ID</returns>
        string Upload(string content);

        /// <summary>
        /// Converts the uploaded part file into a True Hole file by passing in the Settings File ID and Part File ID.
        /// </summary>
        /// <param name="settingsFileID">A settings file ID</param>
        /// <param name="partFileID">A part file ID</param>
        /// <returns>A True Hole file ID</returns>
        string Convert(string settingsFileID, string partFileID);

        /// <summary>
        /// Downloads the True Hole part program by passing the True Hole File ID.
        /// </summary>
        /// <param name="fileID">A True Hole file ID</param>
        /// <returns>A converted part string</returns>
        string Download(string fileID);
    }
}