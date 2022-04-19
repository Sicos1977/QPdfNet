using Newtonsoft.Json;

namespace QPdfNet
{
    /// <summary>
    ///     Pages options
    /// </summary>
    internal class Pages
    {
        #region Fields
        [JsonProperty("file")] private string _file;
        [JsonProperty("range")] private string _range;
        [JsonProperty("password")] private string _password;
        #endregion

        #region Constructor
        /// <summary>
        ///     Makes this object and sets its needed properties
        /// </summary>
        /// <param name="file">The file</param>
        /// <param name="range">The page range</param>
        /// <param name="password">The password or <c>null</c> when not needed</param>
        public Pages(string file, string range, string password = null)
        {
            _file = file;
            _password = password;
            _range = range;
        }
        #endregion
    }
}
