namespace Gefco.CipQuai.Web.Results
{
    /// <summary>
    ///     A class to hold the return code from a Web service call, and associated information as string.
    /// </summary>
    public class StringServiceResult : BaseServiceResult
    {
        /// <summary>
        ///     Gets or sets the data object returned by the Web service.
        /// </summary>
        /// <value>
        ///     The data object returned by the Web service.
        /// </value>
        public string Value { get; set; }
    }
}