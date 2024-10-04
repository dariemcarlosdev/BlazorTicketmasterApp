using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorTicketMasterApp.UnitTests
{
    internal class CustomHttpMessageHandler : HttpMessageHandler
    {
        //This custom HttpMessageHandler class is used to mock the HttpMessageHandler class and expose the SendAsync method as a Func as part of the public properties

        public required Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> SendAsyncFunc { get; set; } //This property is used to expose the SendAsync method as a Func. This is delegate that takes a HttpRequestMessage and a CancellationToken as input and returns a Task<HttpResponseMessage>

        //This method is used to override the SendAsync method of the HttpMessageHandler class
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendAsyncFunc(request, cancellationToken); //This method is used to call the SendAsyncFunc delegate and return the result
        }
    }
}
