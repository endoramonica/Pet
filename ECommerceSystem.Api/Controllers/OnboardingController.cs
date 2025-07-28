using ECommerceSystem.Api.Services;
using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.OnBoarding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingService _onboardingService;

        public OnboardingController(IOnboardingService onboardingService)
        {
            _onboardingService = onboardingService;
        }

        private int? GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : (int?)null;
        }

        // GET: api/onboarding/is-completed
        [HttpGet("is-completed")]
        public async Task<ActionResult<ApiResult<bool>>> IsOnboardingCompleted()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(ApiResult<bool>.Fail("Unauthorized"));

            var result = await _onboardingService.IsOnboardingCompletedAsync(userId.Value);
            return Ok(result);
        }

        // GET: api/onboarding/current-step
        [HttpGet("current-step")]
        public async Task<ActionResult<ApiResult<int>>> GetCurrentStep()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(ApiResult<int>.Fail("Unauthorized"));

            var result = await _onboardingService.GetCurrentStepAsync(userId.Value);
            return Ok(result);
        }

        // POST: api/onboarding/update
        [HttpPost("update")]
        public async Task<ActionResult<ApiResult<bool>>> UpdateOnboarding([FromBody] OnboardingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResult<bool>.Fail("Invalid request data"));
            if (!ModelState.IsValid)
            {
                var errors = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return BadRequest(ApiResult<bool>.Fail($"Invalid request: {errors}"));
            }

            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(ApiResult<bool>.Fail("Unauthorized"));

            request.UserId = userId.Value; // ✅ Tự động gán từ claims, không cần client gửi
            Console.WriteLine("Sending request to onboarding:");
            Console.WriteLine(JsonSerializer.Serialize(request));

            var result = await _onboardingService.UpdateOnboardingAsync(request);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("data")]
        public async Task<ActionResult<ApiResult<OnboardingDataResponse>>> GetOnboardingData()
        {
            var userId = GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(ApiResult<OnboardingDataResponse>.Fail("Unauthorized"));

            var result = await _onboardingService.GetOnboardingDataAsync(userId.Value);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
