// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace Gefco.CipQuai.ApiClient.Models
{
    /// <summary> The PictureServiceResult. </summary>
    public partial class PictureServiceResult
    {
        /// <summary> Initializes a new instance of PictureServiceResult. </summary>
        internal PictureServiceResult()
        {
        }

        /// <summary> Initializes a new instance of PictureServiceResult. </summary>
        /// <param name="value"></param>
        /// <param name="errorCode"></param>
        /// <param name="isSuccess"></param>
        /// <param name="errorMessage"></param>
        internal PictureServiceResult(Picture value, string errorCode, bool? isSuccess, string errorMessage)
        {
            Value = value;
            ErrorCode = errorCode;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
        }

        /// <summary> Gets the value. </summary>
        public Picture Value { get; }
        /// <summary> Gets the error code. </summary>
        public string ErrorCode { get; }
        /// <summary> Gets the is success. </summary>
        public bool? IsSuccess { get; }
        /// <summary> Gets the error message. </summary>
        public string ErrorMessage { get; }
    }
}
