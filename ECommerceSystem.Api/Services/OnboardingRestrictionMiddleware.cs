namespace ECommerceSystem.Api.Services
{
    public class OnboardingRestrictionMiddleware
    {
        private readonly RequestDelegate _next;

        public OnboardingRestrictionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var user = context.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var onboardingClaim = user.FindFirst("IsOnboardingCompleted")?.Value;

                if (onboardingClaim == "false")
                {
                    var path = context.Request.Path.Value?.ToLower();

                    // Chỉ cho phép các đường dẫn onboarding, auth
                    if (!path.StartsWith("/api/onboarding") && !path.StartsWith("/api/auth"))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = "You must complete onboarding before accessing this resource."
                        });
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

}
