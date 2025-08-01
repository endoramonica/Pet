using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerceSystem.GUI.Models
{
    public class PetDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thú cưng không được để trống")]
        [StringLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải là một số không âm")]
        public double Price { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string HealthStatus { get; set; }

        [Required]
        public string Breed { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public PetTypes PetType { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ImageFile { get; set; }

        public bool IsLiked { get; set; }
        public bool IsAdopted { get; set; }
    }

}
