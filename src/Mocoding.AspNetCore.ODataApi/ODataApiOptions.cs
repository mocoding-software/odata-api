namespace Mocoding.AspNetCore.ODataApi
{
    /// <summary>
    /// ODataApi Options
    /// </summary>
    public class ODataApiOptions
    {
        public const string DefaultRoute = "odata";

        public ODataApiOptions()
        {
            RoutePrefix = DefaultRoute;
        }

        /// <summary>
        /// Gets or sets the route prefix.
        /// </summary>
        /// <value>
        /// The route prefix.
        /// </value>
        public string RoutePrefix { get; set; }
    }
}
