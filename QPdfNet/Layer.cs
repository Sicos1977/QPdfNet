using Newtonsoft.Json;

namespace QPdfNet;

/// <summary>
///     Overlay and underlay options
/// </summary>
internal class Layer
{
    #region Fields
    [JsonProperty("to")] private string _to;
    [JsonProperty("from")] private string _from;
    [JsonProperty("repeat")] private string _repeat;
    #endregion

    #region Constructor
    /// <summary>
    ///     Makes this object and sets its needed properties
    /// </summary>
    /// <param name="to">
    ///     Specify a page range (see Page Ranges) that indicates which pages in the output should have the
    ///     overlay/underlay applied. If not specified, overlay/underlay are applied to all pages.
    /// </param>
    /// <param name="from">
    ///     Specify a page range that indicates which pages in the overlay/underlay file will be used for
    ///     overlay or underlay. If not specified, all pages will be used. The “from” pages are used until they are exhausted,
    ///     after which any pages specified with <paramref name="repeat" /> are used. If you are using the
    ///     <paramref name="repeat" /> option, you can use <paramref name="from"/>
    ///     to provide an empty set of “from” pages.
    /// </param>
    /// <param name="repeat">
    ///     Specify an optional page range that indicates which pages in the overlay/underlay file will be
    ///     repeated after the “from” pages are used up. If you want to apply a repeat a range of pages starting with the first
    ///     page of output, you can explicitly use <paramref name="from"/>.
    /// </param>
    public Layer(string to, string from, string repeat = null)
    {
        _to = to;
        _from = from;
        _repeat = repeat;
    }
    #endregion
}