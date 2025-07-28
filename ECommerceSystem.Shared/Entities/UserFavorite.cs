using System;

namespace ECommerceSystem.Shared.Entities
{
    public class UserFavorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        // Navigation
        public User User { get; set; }
        public Pet Pet { get; set; }
    }
}
