namespace ECommerceSystem.GUI.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    public class AuthRetryHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRetryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Bỏ qua đính kèm token nếu là API public
            var path = request.RequestUri?.AbsolutePath ?? string.Empty;
            var isPublicEndpoint = path.Contains("/api/public");

            if (!isPublicEndpoint)
            {
                var token = _httpContextAccessor.HttpContext?.Request?.Cookies["AuthToken"];
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
