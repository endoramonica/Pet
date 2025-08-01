using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedPetsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[]
                {
            "Id", "Name", "Species", "Price", "Location", "HealthStatus",
            "Views", "IsActive", "Breed", "Gender", "AdoptionStatus", "Description",
            "ImageUrl", "IsDeleted", "PetType", "DateOfBirth"
                },
                values: new object[,]
                {
            {
                1001, "Milu", "Dog", 150.0, "Hanoi", "Healthy",
                0, true, "Golden Retriever", 0, 0, "Friendly dog for adoption",
                "https://example.com/milu.jpg", false, 0, new DateTime(2021, 6, 1)
            },
            {
                1002, "Tom", "Cat", 120.0, "HCMC", "Vaccinated",
                0, true, "British Shorthair", 1, 0, "Sweet indoor cat",
                "https://example.com/tom.jpg", false, 1, new DateTime(2022, 3, 15)
            },
            {
                1003, "Max", "Dog", 200.0, "Da Nang", "Healthy",
                0, true, "Labrador", 0, 0, "Energetic and playful",
                "https://example.com/max.jpg", false, 0, new DateTime(2020, 8, 12)
            },
            {
                1004, "Luna", "Cat", 130.0, "Can Tho", "Dewormed",
                0, true, "Siamese", 1, 0, "Loves cuddles and naps",
                "https://example.com/luna.jpg", false, 1, new DateTime(2021, 11, 5)
            },
            {
                1005, "Buddy", "Dog", 170.0, "Hue", "Healthy",
                0, true, "Beagle", 0, 0, "Great with kids",
                "https://example.com/buddy.jpg", false, 0, new DateTime(2019, 3, 22)
            },
            {
                1006, "Mimi", "Cat", 110.0, "Nha Trang", "Vaccinated",
                0, true, "Persian", 1, 0, "Fluffy and calm",
                "https://example.com/mimi.jpg", false, 1, new DateTime(2022, 2, 10)
            },
            {
                1007, "Rocky", "Dog", 160.0, "Hanoi", "Healthy",
                0, true, "Husky", 0, 0, "Strong and loyal",
                "https://example.com/rocky.jpg", false, 0, new DateTime(2020, 10, 1)
            },
            {
                1008, "Neko", "Cat", 105.0, "HCMC", "Healthy",
                0, true, "Maine Coon", 1, 0, "Large and friendly",
                "https://example.com/neko.jpg", false, 1, new DateTime(2021, 1, 25)
            },
            {
                1009, "Charlie", "Dog", 180.0, "Da Nang", "Dewormed",
                0, true, "Corgi", 0, 0, "Short legs, big heart",
                "https://example.com/charlie.jpg", false, 0, new DateTime(2021, 9, 9)
            },
            {
                1010, "Kitty", "Cat", 95.0, "Hue", "Healthy",
                0, true, "Ragdoll", 1, 0, "Very soft and quiet",
                "https://example.com/kitty.jpg", false, 1, new DateTime(2022, 5, 12)
            }
                });
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int id = 1001; id <= 1010; id++)
            {
                migrationBuilder.DeleteData(
                    table: "Pets",
                    keyColumn: "Id",
                    keyValue: id);
            }
        }
    }
}
