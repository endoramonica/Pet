using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.OnBoarding;
using Refit;

namespace ECommerceSystem.GUI.Apis
{
    public interface IOnboardingApi
    {
        [Get("/api/onboarding/is-completed")]
        Task<ApiResult<bool>> IsOnboardingCompletedAsync();

        [Get("/api/onboarding/current-step")]
        Task<ApiResult<OnboardingStep>> GetCurrentStepAsync();


        [Post("/api/onboarding/update")]
        Task<ApiResult<bool>> UpdateOnboardingAsync([Body] OnboardingRequest request);
        [Get("/api/onboarding/data")]
        Task<ApiResult<OnboardingDataResponse>> GetOnboardingDataAsync();

    }
}
