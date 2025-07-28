using ECommerceSystem.Shared.Enums;
using System;

namespace ECommerceSystem.Shared.DTOs.Pet
{
    public class PetDetailDto : PetListDto
    {
        public bool IsFavorite { get; set; } // Đánh dấu yêu thích
        public Gender Gender { get; set; } // Giới tính thú cưng
        public AdoptionStatus AdoptionStatus { get; set; } // Trạng thái nhận nuôi
        public string Description { get; set; } // Mô tả chi tiết thú cưng
        public string ImageUrl { get; set; } // URL hình ảnh chi tiết
        public DateTime DateOfBirth { get; set; } // Ngày giờ tạo bản ghi
        public string GenderDisplay => Gender.ToString(); // Hiển thị giới tính thú cưng
        public string GenderImage => Gender switch { Gender.Male => "male" , Gender.Female => "female"};
       public string Age
{
    get
    {
        var diff = DateTime.Now.Date - DateOfBirth.Date;

        return diff.TotalDays switch
        {
            < 30 => $"{(int)diff.TotalDays} days",
            >= 30 and <= 60 => "1 month",
            < 365 => $"{(int)(diff.TotalDays / 30)} months",
            _ => $"{(int)(diff.TotalDays / 365)} years"
        };
    }
}


        public string AdoptionHistory { get; set; } // Lịch sử nhận nuôi
        public string MedicalRecords { get; set; } // Hồ sơ y tế
        public string OwnerInfo { get; set; } // Thông tin chủ sở hữu hiện tại
        // Có thể bổ sung các trường chi tiết khác nếu cần
    }
}
