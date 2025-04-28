using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace MyChat.Jwt
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;
        public AuthHeaderHandler(IJSRuntime js) => _js = js;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken ct)
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (!string.IsNullOrWhiteSpace(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, ct);
        }
    }
}
