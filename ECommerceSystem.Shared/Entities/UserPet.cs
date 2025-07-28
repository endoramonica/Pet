namespace ECommerceSystem.Shared.Entities
{
    // Thực thể trung gian giữa User và Pet
    public class UserPet
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }

        // Thêm các thuộc tính khác nếu cần, ví dụ: DateAdded, IsOwner, v.v.
    }
}