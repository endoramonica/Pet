using ECommerceSystem.Shared.Enums;
using System;

namespace ECommerceSystem.Shared.DTOs.Pet
{
    public class PetListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Location { get; set; }
        public string HealthStatus { get; set; }
        public int Views { get; set; }
        public bool IsActive { get; set; }
        public string Breed { get; set; }
        public int Age { get; set; }
        public string ImageUrl { get; set; }
        public PetTypes PetType { get; set; }
    }
}
