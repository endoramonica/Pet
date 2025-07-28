using System.Security.Claims;

namespace ECommerceSystem.Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                var claims = user.Claims.Select(c => $"{c.Type} = {c.Value}");
                var debug = string.Join("\n", claims);
                throw new InvalidOperationException("User ID claim not found.\nAvailable claims:\n" + debug);
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
                throw new InvalidOperationException("User ID claim is not a valid integer.");

            return userId;
        }



        public static bool TryGetUserId(this ClaimsPrincipal user, out int userId)
        {
            userId = 0;
            if (user == null) return false;

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return false;

            return int.TryParse(userIdClaim.Value, out userId);
        }
    }
}
