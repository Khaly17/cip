using System;
using Newtonsoft.Json;

namespace Gefco.CipQuai.Web.Results
{
    public class BaseServiceResult
    {
        public BaseServiceResult()
        {

        }

        public BaseServiceResult(Exception ex)
        {
            SetError(ex);
        }
        /// <summary>
        ///     Gets or sets the error code.
        /// </summary>
        /// <value>
        ///     The error code.
        /// </value>
        public string ErrorCode { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance's call is success.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance's call is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; }
        /// <summary>
        ///     Gets or sets the error message.
        /// </summary>
        /// <value>
        ///     The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        ///     Set the error message, error code and errorid, and log the error
        /// </summary>
        /// <param name="e">The exception that occured</param>
        /// <param name="defaultMessage">The message that will be sent if no error details are returned</param>
        /// <param name="errorCode">The error code that will be returned</param>
        public virtual void SetError(Exception e, string defaultMessage, string errorCode)
        {
            IsSuccess = false;
            ErrorCode = errorCode;
            if (e != null)
                ErrorMessage = string.Format("[{2}] {0}: {1}", defaultMessage, e.Message, errorCode);
            else
                ErrorMessage = defaultMessage;

            SimpleLogger.GetOne().LogMessage($"{errorCode}: {defaultMessage}", SimpleLogger.ErrorLevel.Error);
            if (e != null) SimpleLogger.GetOne().Error(e);
        }
        public virtual void SetError(Exception e)
        {
            IsSuccess = false;
            ErrorCode = Guid.NewGuid().ToString();
            ErrorMessage = string.Format("[{1}]  {0}", e, ErrorCode);

            SimpleLogger.GetOne().LogMessage($"[{ErrorCode}]", SimpleLogger.ErrorLevel.Error);
            if (e != null) SimpleLogger.GetOne().Error(e);
        }

    }
}