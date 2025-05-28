namespace Gefco.CipQuai.Web.Results
{
    /// <summary>
    ///     A base class to hold the return code from a Web service call, and associated information.
    /// </summary>
    public class IntServiceResult : BaseServiceResult
    {
        /// <summary>
        ///     Gets or sets the data object returned by the Web service.
        /// </summary>
        /// <value>
        ///     The data object returned by the Web service.
        /// </value>
        public int Value { get; set; }
    }
}