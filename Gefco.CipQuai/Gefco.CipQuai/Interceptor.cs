using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.Rest;

namespace Gefco.CipQuai
{
    public class Interceptor : IServiceClientTracingInterceptor
    {
        public void Information(string message)
        {
            Debug.WriteLine("********************************************************");
            Debug.WriteLine("********************* INFO *****************************");
            Debug.WriteLine(message);
            Debug.WriteLine("********************************************************");
        }

        public void Configuration(string source, string name, string value)
        {
            Debug.WriteLine("**********************************************************");
            Debug.WriteLine("*********************** CONFIG ***************************");
            Debug.WriteLine($"Source : {source}");
            Debug.WriteLine("**********************************************************");
        }

        public void EnterMethod(string invocationId, object instance, string method,
            IDictionary<string, object> parameters)
        {
        }

        public void TraceError(string invocationId, Exception exception)
        {
            Debug.WriteLine("********************************************************");
            Debug.WriteLine("********************* ERROR ****************************");
            Debug.WriteLine(exception);
            Debug.WriteLine("********************************************************");
        }

        public void ExitMethod(string invocationId, object returnValue)
        {
        }

        public void ReceiveResponse(string invocationId, HttpResponseMessage response)
        {
            Debug.WriteLine("********************************************************");
            Debug.WriteLine("********************** RESP ****************************");
            Debug.WriteLine("Code : " + response.StatusCode);
            Debug.WriteLine("URI : " + response.RequestMessage.RequestUri);
            Debug.WriteLine(response.AsFormattedString());
            Debug.WriteLine("********************************************************");
        }

        public void SendRequest(string invocationId, HttpRequestMessage request)
        {
            Debug.WriteLine("********************************************************");
            Debug.WriteLine("********************** REQU ****************************");
            Debug.WriteLine(request.RequestUri);
            Debug.WriteLine(request.AsFormattedString());
            Debug.WriteLine("********************************************************");
        }
    }
}