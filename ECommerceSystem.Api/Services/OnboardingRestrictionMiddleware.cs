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
                var role = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

                var isAdmin = role == "Admin"; // ✅ Loại trừ Admin
                var isOnboardingIncomplete = onboardingClaim == "false";

                if (isOnboardingIncomplete && !isAdmin)
                {
                    var path = context.Request.Path.Value?.ToLower();

                    // Chỉ cho phép đi qua các API liên quan tới onboarding hoặc auth
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
