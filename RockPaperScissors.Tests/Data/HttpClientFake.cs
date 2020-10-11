using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace RockPaperScissors.Tests.Data
{
    public class HttpClientFake : HttpClient
    {
        public HttpClientFake() : base(new HttpMessageHandlerFake())
        {
            BaseAddress = new Uri("https://www.google.com");
        }
    }

    public class HttpMessageHandlerFake : HttpMessageHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        
        {
            if (request.RequestUri.AbsolutePath.Contains("/validate/rock-win"))
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"isPlayerSelectionValid\":true,\"playerChoice\":1,\"computerChoice\":0,\"gameResult\":null,\"errorMessage\":null}")
                };
            }
            //Add more test scenarios here.
            else if (request.RequestUri.AbsolutePath.Contains("/play"))
            {
                string content = await request.Content.ReadAsStringAsync();
                if (content == "{\"isPlayerSelectionValid\":true,\"playerChoice\":1,\"computerChoice\":0,\"gameResult\":null,\"errorMessage\":null}")
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent("{\"isPlayerSelectionValid\":true,\"playerChoice\":1,\"computerChoice\":3,\"gameResult\":\"Congratulations, you won!\",\"errorMessage\":null}")
                    };
                }
            }
            else
            {
                return new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{\"isPlayerSelectionValid\":false,\"playerChoice\":0,\"computerChoice\":0,\"gameResult\":null,\"errorMessage\":\"Unknown failure\"}")
                };
            }

            return null;
        }
    }
}