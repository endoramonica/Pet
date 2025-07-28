using ECommerceSystem.GUI.Apis;
using ECommerceSystem.Shared.DTOs.Models;
using ECommerceSystem.Shared.DTOs.OnBoarding;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Refit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AuthService 
{
    private readonly IAuthApi _authApi;
    private readonly IOnboardingApi _onboardingApi;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthApi authApi, IOnboardingApi onboardingApi,
        IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger)
    {
        _authApi = authApi;
        _onboardingApi = onboardingApi;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<(bool Success, string Token, string Role)> LoginAsync(LoginModel model)
    {
        try
        {
            var response = await _authApi.Login(model);

            if (string.IsNullOrWhiteSpace(response.Token) || string.IsNullOrEmpty(response.Role))
                return (false, null, null);

            SaveTokenToCookie(response.Token);
            await SignInJwtTokenAsync(response.Token);

            return (true, response.Token, response.Role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user: {User}", model.Username);
            return (false, null, null);
        }
    }

    public async Task<bool> IsOnboardingCompletedAsync()
    {
        var result = await _onboardingApi.IsOnboardingCompletedAsync();
        return result.Success && result.Data;
    }

    public async Task<OnboardingStep> GetCurrentStepAsync()
    {
        var result = await _onboardingApi.GetCurrentStepAsync();
        return result.Data;
    }


    public async Task<(bool Success, string Message)> RegisterAsync(RegisterModel model)
    {
        try
        {
            var response = await _authApi.Register(model);
            if (response == null)
                return (false, "Không nhận được phản hồi từ API.");

            return (true, "Đăng ký thành công.");
        }
        catch (ApiException ex)
        {
            return (false, $"Lỗi API: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, $"Lỗi hệ thống: {ex.Message}");
        }
    }


    public string GetRoleFromToken()
    {
        var token = GetTokenFromCookie();
        if (string.IsNullOrEmpty(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        return jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }

    public void Logout()
    {
        _httpContextAccessor.HttpContext?.SignOutAsync();
        _httpContextAccessor.HttpContext?.Response?.Cookies.Delete("AuthToken");
    }

    private async Task SignInJwtTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        var identity = new ClaimsIdentity(jwtToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContextAccessor.HttpContext.SignInAsync(
            "MyCookieAuth",principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3)
            });
    }

    private void SaveTokenToCookie(string token)
    {
        _httpContextAccessor.HttpContext?.Response?.Cookies.Append("AuthToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(3) // Đồng bộ với authentication
        });
    }
    private string GetTokenFromCookie()
    {
        return _httpContextAccessor.HttpContext?.Request?.Cookies["AuthToken"];
    }
}
