using ECommerceSystem.Shared.DTOs.User;
using ECommerceSystem.Api.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceSystem.Api.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // [GET] /api/admin/users
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // [GET] /api/admin/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            if (!int.TryParse(id, out var userId))
                return BadRequest("ID không hợp lệ");

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // [PUT] /api/admin/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserDTO dto)
        {
            if (!int.TryParse(id, out var userId))
                return BadRequest("ID không hợp lệ");

            if (id != dto.Id)
                return BadRequest("ID không khớp");

            await _userService.UpdateAsync(userId, dto);
            return NoContent();
        }

        // [DELETE] /api/admin/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(string id)
        {
            if (!int.TryParse(id, out var userId))
                return BadRequest("ID không hợp lệ");

            await _userService.SoftDeleteAsync(userId);
            return NoContent();
        }

        // [POST] /api/admin/users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDTO dto)
        {
            await _userService.CreateAsync(dto);
            return Ok();
        }

        // [GET] /api/admin/users/search?name=abc
        [HttpGet("search")]
        public async Task<ActionResult<List<UserDTO>>> SearchByName([FromQuery] string name)
        {
            var users = await _userService.SearchByNameAsync(name);
            return Ok(users);
        }

        // [POST] /api/admin/users/delete-multiple
        [HttpPost("delete-multiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<string> ids)
        {
            var idInts = new List<int>();
            foreach (var id in ids)
            {
                if (int.TryParse(id, out var intId))
                    idInts.Add(intId);
                else
                    return BadRequest($"ID không hợp lệ: {id}");
            }

            await _userService.SoftDeleteMultipleAsync(idInts);
            return NoContent();
        }
    }
}
