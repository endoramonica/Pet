using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.OnBoarding;
using ECommerceSystem.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceSystem.Api.Services
{
    public class OnboardingService : IOnboardingService
    {
        private readonly WebDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OnboardingService(WebDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }

        public async Task<ApiResult<bool>> IsOnboardingCompletedAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return ApiResult<bool>.Fail("User not found");

            return ApiResult<bool>.Ok(user.IsOnboardingCompleted);
        }

        public async Task<ApiResult<int>> GetCurrentStepAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return ApiResult<int>.Fail("User not found");

            int step;
            if (string.IsNullOrEmpty(user.InterestsJson))
                step = (int)OnboardingStep.SelectInterests;
            else if (string.IsNullOrEmpty(user.Name))
                step = (int)OnboardingStep.SetupProfile;
            else if (!user.IsOnboardingCompleted)
                step = (int)OnboardingStep.Confirm;
            else
                step = (int)OnboardingStep.Confirm + 1;

            return ApiResult<int>.Ok(step);
        }
        public async Task<ApiResult<OnboardingDataResponse>> GetOnboardingDataAsync(int userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                return ApiResult<OnboardingDataResponse>.Fail("User not found");

            var interests = string.IsNullOrEmpty(user.InterestsJson)
                ? new List<string>()
                : user.InterestsJson.Split(',').ToList();

            var response = new OnboardingDataResponse
            {
                AccountName = user.Name,
               // ProfilePhotoUrl = user.UpdatedAt != null ? user.ProfilePhotoUrl : null,
                SelectedInterests = interests
            };

            return ApiResult<OnboardingDataResponse>.Ok(response);
        }

        public async Task<ApiResult<bool>> UpdateOnboardingAsync(OnboardingRequest request)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
                return ApiResult<bool>.Fail("User is not authenticated");

            var user = await _dbContext.Users.FindAsync(userId.Value);
            if (user == null)
                return ApiResult<bool>.Fail("User not found");

            var updated = false;

            switch (request.CurrentStep)
            {
                case OnboardingStep.SelectInterests:
                    if (request.SelectedInterests?.Any() == true)
                    {
                        user.InterestsJson = string.Join(",", request.SelectedInterests);
                        updated = true;
                    }
                    break;

                case OnboardingStep.SetupProfile:
                    if (!string.IsNullOrWhiteSpace(request.AccountName))
                    {
                        user.Name = request.AccountName;
                        updated = true;
                    }
                    break;

                case OnboardingStep.Confirm:
                    user.IsOnboardingCompleted = true;
                    updated = true;
                    break;
            }

            if (!updated)
                return ApiResult<bool>.Fail("No valid data to update");

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return ApiResult<bool>.Ok(true, "Onboarding updated successfully");
        }

    }

    public interface IOnboardingService
    {
        Task<ApiResult<bool>> IsOnboardingCompletedAsync(int userId);
        Task<ApiResult<int>> GetCurrentStepAsync(int userId);
        Task<ApiResult<bool>> UpdateOnboardingAsync(OnboardingRequest request);
        Task<ApiResult<OnboardingDataResponse>> GetOnboardingDataAsync(int userId);

    }
}
