using ECommerceSystem.Shared.Enums;
using System;
using System.Collections.Generic;

namespace ECommerceSystem.Shared.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public double Price { get; set; } // Giá nhận nuôi
        public string Location { get; set; } // Vị trí hiện tại của thú cưng
        public string HealthStatus { get; set; } // Tình trạng sức khỏe
        public int Views { get; set; } // Số lượt xem thú cưng
        public bool IsActive { get; set; } = true; // Trạng thái có sẵn để nhận nuôi
        public string Breed { get; set; }
        public PetTypes PetType { get; set; } // Loại thú cưng (Dog, Cat, etc.)
        public Gender Gender { get; set; }
        public AdoptionStatus AdoptionStatus { get; set; } = AdoptionStatus.Available; // Trạng thái nhận nuôi (Pending, Available, Adopted)
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Quan hệ với UserAdoption, UserFavorite   
        public ICollection<UserPet> UserPets { get; set; }
        public ICollection<UserAdoption> UserAdoptions { get; set; }
        public ICollection<UserFavorite> UserFavorites { get; set; }
    }
}
