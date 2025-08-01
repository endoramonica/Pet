using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.DTOs.Product;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerceSystem.GUI.Models
{
    public class ProductFormModel
    {
        public ProductDTO Product { get; set; } = new ProductDTO();

        [Display(Name = "Ảnh đại diện")]
        public IFormFile? ThumbnailFile { get; set; }
    }
    public class NewFeedViewModel
    {
        public List<PetListDto> NewPets { get; set; } = new();
        public List<PetListDto> PopularPets { get; set; } = new();
        public List<PetListDto> RandomPets { get; set; } = new();
    }
}
