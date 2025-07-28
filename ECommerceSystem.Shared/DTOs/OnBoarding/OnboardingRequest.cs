using System;
using System.Collections.Generic;

namespace ECommerceSystem.Shared.DTOs.OnBoarding
{
    public class OnboardingRequest

    {
        public int UserId { get; set; } // Không cần truyền từ client, sẽ lấy từ claims
        // Không cần truyền UserId từ client
        public OnboardingStep CurrentStep { get; set; }

        // Áp dụng cho bước chọn sở thích
        public List<string> SelectedInterests { get; set; } = new();

        // Áp dụng cho bước thiết lập hồ sơ
        public string AccountName { get; set; }
        //public string ProfilePhotoUrl { get; set; }

    }

    public class OnboardingStatusResponse
    {
        public int UserId { get; set; }
        public bool IsOnboardingCompleted { get; set; }
    }
    public class OnboardingDataResponse
    {
        public string AccountName { get; set; }
       // public string ProfilePhotoUrl { get; set; }
        public List<string> SelectedInterests { get; set; }
    }
    public class OnboardingStepResponse
    {
        public int UserId { get; set; }

        public OnboardingStep CurrentStep { get; set; }
    }

    public enum OnboardingStep
    {
        Intro = 0,
        SelectInterests = 1,
        SetupProfile = 2,
        Confirm = 3
    }
}
