namespace Gefco.CipQuai.Web.Results
{
    /// <summary>
    ///     A base, abstract class to hold the return data from a Web service call, derived from
    ///     <see cref="BaseServiceResult" />.
    /// </summary>
    /// <typeparam name="T">A class holding the actual data returned from a Web service call.</typeparam>
    public abstract class ServiceResult<T> : BaseServiceResult
        where T : class
    {
        private T _value;

        /// <summary>
        ///     Gets or sets the data object returned by the Web service.
        /// </summary>
        /// <value>
        ///     The data object returned by the Web service.
        /// </value>
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                IsSuccess = true;
            }
        }
    }
}