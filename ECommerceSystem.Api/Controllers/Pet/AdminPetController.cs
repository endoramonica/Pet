using ECommerceSystem.Api.Services;
using ECommerceSystem.Shared.DTOs.Pet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Api.Controllers.Pet
{
    [Route("api/admin/pets")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Chỉ có Admin mới được truy cập
    public class AdminPetController : ControllerBase
    {
        private readonly IPetService _petService;

        public AdminPetController(IPetService petService)
        {
            _petService = petService;
        }

        // GET: api/admin/pets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _petService.GetAllPetsForAdminAsync();
            return Ok(result);
        }

        // GET: api/admin/pets/for-update/{id}
        [HttpGet("for-update/{id:int}")]
        public async Task<IActionResult> GetForUpdate(int id)
        {
            var result = await _petService.GetPetForUpdateAsync(id);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }

        // POST: api/admin/pets
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PetCreateUpdateDto petDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _petService.CreatePetAsync(petDto);
            if (!result.Success) return BadRequest(result);
            // Trả về ID của pet mới được tạo
            return CreatedAtAction(nameof(GetForUpdate), new { id = result.Data }, result);
        }

        // PUT: api/admin/pets/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PetCreateUpdateDto petDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _petService.UpdatePetAsync(id, petDto);
            if (!result.Success) return NotFound(result);
            return NoContent(); // Thành công, không cần trả về nội dung
        }

        // DELETE: api/admin/pets/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _petService.DeletePetAsync(id);
            if (!result.Success) return NotFound(result);
            return NoContent();
        }
    }
}
