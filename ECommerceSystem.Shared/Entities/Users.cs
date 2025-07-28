using Microsoft.AspNetCore.Identity;

namespace ECommerceSystem.Shared.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string DeviceToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string InterestsJson { get; set; }
        public bool IsOnboardingCompleted { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Quan hệ với UserPet, UserFavorite, UserAdoption
        public ICollection<UserPet> UserPets { get; set; } = new List<UserPet>();
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
        public ICollection<UserAdoption> UserAdoptions { get; set; } = new List<UserAdoption>();
    }

    public class Role : IdentityRole<int>
    {
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}