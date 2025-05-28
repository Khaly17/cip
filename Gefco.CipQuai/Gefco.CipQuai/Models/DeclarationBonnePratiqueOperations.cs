using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Gefco.CipQuai.ApiClient.Models;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Gefco.CipQuai.ApiClient
{
    public partial class DeclarationBonnePratiqueOperations
    {
        /// <param name='appVersion'>
        /// </param>
        /// <param name='id'>
        /// </param>
        /// <param name='pictureType'>
        /// </param>
        /// <param name='declarationId'>
        /// </param>
        /// <param name='fileName'>
        /// </param>
        /// <param name='file'>
        /// Upload picture
        /// </param>
        /// <param name='customHeaders'>
        /// Headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when a required parameter is null
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public async Task<HttpOperationResponse<PictureServiceResult>> UploadPictureBPWithHttpMessagesAsync(string appVersion, string id, int pictureType, string declarationId, string fileName, Stream file, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (appVersion == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "appVersion");
            }
            if (id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "id");
            }
            if (declarationId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "declarationId");
            }
            if (fileName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "fileName");
            }
            if (file == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "file");
            }
            // Tracing
            bool _shouldTrace = ServiceClientTracing.IsEnabled;
            string _invocationId = null;
            if (_shouldTrace)
            {
                _invocationId = ServiceClientTracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("appVersion", appVersion);
                tracingParameters.Add("id", id);
                tracingParameters.Add("pictureType", pictureType);
                tracingParameters.Add("declarationId", declarationId);
                tracingParameters.Add("fileName", fileName);
                tracingParameters.Add("file", file);
                tracingParameters.Add("cancellationToken", cancellationToken);
                ServiceClientTracing.Enter(_invocationId, this, "UploadPictureBP", tracingParameters);
            }
            // Construct URL
            var _baseUrl = Client.BaseUri.AbsoluteUri;
            var _url = new System.Uri(new System.Uri(_baseUrl
                                                     + (_baseUrl.EndsWith("/")
                                                            ? ""
                                                            : "/")), "UploadPictureBP").ToString();
            List<string> _queryParameters = new List<string>();
            if (appVersion != null)
            {
                _queryParameters.Add(string.Format("appVersion={0}", System.Uri.EscapeDataString(appVersion)));
            }
            if (id != null)
            {
                _queryParameters.Add(string.Format("id={0}", System.Uri.EscapeDataString(id)));
            }
            _queryParameters.Add(string.Format("pictureType={0}", System.Uri.EscapeDataString(Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(pictureType, Client.SerializationSettings).Trim('"'))));
            if (declarationId != null)
            {
                _queryParameters.Add(string.Format("declarationId={0}", System.Uri.EscapeDataString(declarationId)));
            }
            if (fileName != null)
            {
                _queryParameters.Add(string.Format("fileName={0}", System.Uri.EscapeDataString(fileName)));
            }
            if (_queryParameters.Count > 0)
            {
                _url += "?" + string.Join("&", _queryParameters);
            }
            // Create HTTP transport objects
            var _httpRequest = new HttpRequestMessage();
            HttpResponseMessage _httpResponse = null;
            _httpRequest.Method = new HttpMethod("PUT");
            _httpRequest.RequestUri = new System.Uri(_url);
            // Set Headers

            if (customHeaders != null)
            {
                foreach (var _header in customHeaders)
                {
                    if (_httpRequest.Headers.Contains(_header.Key))
                    {
                        _httpRequest.Headers.Remove(_header.Key);
                    }
                    _httpRequest.Headers.TryAddWithoutValidation(_header.Key, _header.Value);
                }
            }

            // Serialize Request
            string _requestContent = null;
            var content = new MultipartFormDataContent();
            if (file != null)
            {
                //values.Add(new KeyValuePair<string,string>("File", file));
                var streamContent = new StreamContent(file);
                streamContent.Headers.Add("Content-Type", "application/octet-stream");
                streamContent.Headers.Add("Content-Disposition", "form-data; name=File; filename=" + fileName + "");
                content.Add(streamContent, "File", fileName);
            }
            _httpRequest.Content = content;
            // Send Request
            if (_shouldTrace)
            {
                ServiceClientTracing.SendRequest(_invocationId, _httpRequest);
            }
            cancellationToken.ThrowIfCancellationRequested();
            _httpResponse = await Client.HttpClient.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
            if (_shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(_invocationId, _httpResponse);
            }
            HttpStatusCode _statusCode = _httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string _responseContent = null;
            if ((int) _statusCode != 200)
            {
                var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", _statusCode));
                if (_httpResponse.Content != null)
                {
                    _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else
                {
                    _responseContent = string.Empty;
                }
                ex.Request = new HttpRequestMessageWrapper(_httpRequest, _requestContent);
                ex.Response = new HttpResponseMessageWrapper(_httpResponse, _responseContent);
                if (_shouldTrace)
                {
                    ServiceClientTracing.Error(_invocationId, ex);
                }
                _httpRequest.Dispose();
                if (_httpResponse != null)
                {
                    _httpResponse.Dispose();
                }
                throw ex;
            }
            // Create Result
            var _result = new HttpOperationResponse<PictureServiceResult>();
            _result.Request = _httpRequest;
            _result.Response = _httpResponse;
            // Deserialize Response
            if ((int) _statusCode == 200)
            {
                _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    _result.Body = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<PictureServiceResult>(_responseContent, Client.DeserializationSettings);
                }
                catch (JsonException ex)
                {
                    _httpRequest.Dispose();
                    if (_httpResponse != null)
                    {
                        _httpResponse.Dispose();
                    }
                    throw new SerializationException("Unable to deserialize the response.", _responseContent, ex);
                }
            }
            if (_shouldTrace)
            {
                ServiceClientTracing.Exit(_invocationId, _result);
            }
            return _result;
        }
        /// <param name='appVersion'>
        /// </param>
        /// <param name='id'>
        /// </param>
        /// <param name='pictureType'>
        /// </param>
        /// <param name='declarationId'>
        /// </param>
        /// <param name='fileName'>
        /// </param>
        /// <param name='fileContent'>
        /// </param>
        /// <param name='customHeaders'>
        /// Headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when a required parameter is null
        /// </exception>
        /// <return>
        /// A response object containing the response body and response headers.
        /// </return>
        public async Task<HttpOperationResponse<PictureServiceResult>> UploadPicBPWithHttpMessagesAsync(string appVersion, string id, int pictureType, string declarationId, string fileName, string fileContent, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (appVersion == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "appVersion");
            }
            if (id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "id");
            }
            if (declarationId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "declarationId");
            }
            if (fileName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "fileName");
            }
            if (fileContent == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "fileContent");
            }
            // Tracing
            bool _shouldTrace = ServiceClientTracing.IsEnabled;
            string _invocationId = null;
            if (_shouldTrace)
            {
                _invocationId = ServiceClientTracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("appVersion", appVersion);
                tracingParameters.Add("id", id);
                tracingParameters.Add("pictureType", pictureType);
                tracingParameters.Add("declarationId", declarationId);
                tracingParameters.Add("fileName", fileName);
                tracingParameters.Add("fileContent", fileContent);
                tracingParameters.Add("cancellationToken", cancellationToken);
                ServiceClientTracing.Enter(_invocationId, this, "UploadPicBP", tracingParameters);
            }
            // Construct URL
            var _baseUrl = Client.BaseUri.AbsoluteUri;
            var _url = new System.Uri(new System.Uri(_baseUrl
                                                     + (_baseUrl.EndsWith("/")
                                                            ? ""
                                                            : "/")), "UploadPicBP").ToString();
            // Create HTTP transport objects
            var _httpRequest = new HttpRequestMessage();
            HttpResponseMessage _httpResponse = null;
            _httpRequest.Method = new HttpMethod("POST");
            _httpRequest.RequestUri = new System.Uri(_url);
            // Set Headers

            if (customHeaders != null)
            {
                foreach (var _header in customHeaders)
                {
                    if (_httpRequest.Headers.Contains(_header.Key))
                    {
                        _httpRequest.Headers.Remove(_header.Key);
                    }
                    _httpRequest.Headers.TryAddWithoutValidation(_header.Key, _header.Value);
                }
            }

            // Serialize Request
            string _requestContent = null;
            {
                _requestContent = Microsoft.Rest.Serialization.SafeJsonConvert.SerializeObject(new PictureParameter()
                                                                                               {
                                                                                                   AppVersion = appVersion,
                                                                                                   FileContent = fileContent,
                                                                                                   FileName = fileName,
                                                                                                   Id = id,
                                                                                                   DeclarationId = declarationId,
                                                                                                   PictureType = (PictureType)pictureType
                                                                                               }, Client.SerializationSettings);
                _httpRequest.Content = new StringContent(_requestContent, System.Text.Encoding.UTF8);
                _httpRequest.Content.Headers.ContentType =System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
            }
            // Send Request
            if (_shouldTrace)
            {
                ServiceClientTracing.SendRequest(_invocationId, _httpRequest);
            }
            cancellationToken.ThrowIfCancellationRequested();
            _httpResponse = await Client.HttpClient.SendAsync(_httpRequest, cancellationToken).ConfigureAwait(false);
            if (_shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(_invocationId, _httpResponse);
            }
            HttpStatusCode _statusCode = _httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string _responseContent = null;
            if ((int) _statusCode != 200)
            {
                var ex = new HttpOperationException(string.Format("Operation returned an invalid status code '{0}'", _statusCode));
                if (_httpResponse.Content != null)
                {
                    _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else
                {
                    _responseContent = string.Empty;
                }
                ex.Request = new HttpRequestMessageWrapper(_httpRequest, _requestContent);
                ex.Response = new HttpResponseMessageWrapper(_httpResponse, _responseContent);
                if (_shouldTrace)
                {
                    ServiceClientTracing.Error(_invocationId, ex);
                }
                _httpRequest.Dispose();
                if (_httpResponse != null)
                {
                    _httpResponse.Dispose();
                }
                throw ex;
            }
            // Create Result
            var _result = new HttpOperationResponse<PictureServiceResult>();
            _result.Request = _httpRequest;
            _result.Response = _httpResponse;
            // Deserialize Response
            if ((int) _statusCode == 200)
            {
                _responseContent = await _httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    _result.Body = Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<PictureServiceResult>(_responseContent, Client.DeserializationSettings);
                }
                catch (JsonException ex)
                {
                    _httpRequest.Dispose();
                    if (_httpResponse != null)
                    {
                        _httpResponse.Dispose();
                    }
                    throw new SerializationException("Unable to deserialize the response.", _responseContent, ex);
                }
            }
            if (_shouldTrace)
            {
                ServiceClientTracing.Exit(_invocationId, _result);
            }
            return _result;
        }
    }
}
