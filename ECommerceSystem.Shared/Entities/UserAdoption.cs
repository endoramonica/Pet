using System;

namespace ECommerceSystem.Shared.Entities
{
    public class UserAdoption
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PetId { get; set; }
        public DateTime AdoptionDate { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public bool IsDeleted { get; set; } = false;
        // Navigation
        public User User { get; set; }
        public Pet Pet { get; set; }
    }
}
