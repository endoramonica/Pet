namespace ECommerceSystem.Shared.DTOs.User
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string DeviceToken { get; set; }
        public bool IsDeleted { get; set; }

        // Add these properties:
        public string ProfilePhotoUrl { get; set; }
        public List<string> SelectedInterests { get; set; }
        public bool IsOnboardingCompleted { get; set; }
        public DateTime? OnboardingCompletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}

