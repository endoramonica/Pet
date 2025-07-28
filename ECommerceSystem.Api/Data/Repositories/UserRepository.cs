using DocumentFormat.OpenXml.Spreadsheet;
using ECommerceSystem.Api.Data;
using ECommerceSystem.Shared.DTOs.User;
using ECommerceSystem.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceSystem.Api.Data.Repositories
{
    public class UserRepository
    {
        private readonly WebDBContext _dbContext;

        public UserRepository(WebDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDTO> GetUserInfo(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username && !u.IsDeleted);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                DeviceToken = user.DeviceToken,
                IsDeleted = user.IsDeleted
            };
        }

        public async Task<(UserDTO User, string Role)> GetUserInfoAndRole(string username)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == username && !u.IsDeleted);

            if (user == null) return (null, null);

            return (new UserDTO
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                DeviceToken = user.DeviceToken,
                IsDeleted = user.IsDeleted
            }, user.Role?.Name);
        }

        public async Task<User> GetUserPasswordHash(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username && !u.IsDeleted);
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }

        public async Task CreateRoleAsync(Role role)
        {
            await _dbContext.Roles.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateUserAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            var role = await GetRoleByName(roleName);
            if (role == null)
            {
                throw new System.Exception($"Vai trò '{roleName}' không tồn tại.");
            }

            user.RoleId = role.Id;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserPasswordHashById(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            return await _dbContext.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserDTO
                {
                    Id = u.Id.ToString(),
                    Email = u.Email,
                    Name = u.Name,
                    DeviceToken = u.DeviceToken,
                    IsDeleted = u.IsDeleted
                }).ToListAsync();
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Name = user.Name,
                DeviceToken = user.DeviceToken,
                IsDeleted = user.IsDeleted
            };
        }

        public async Task UpdateAsync(int id, UserDTO dto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) throw new System.Exception("Người dùng không tồn tại.");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.DeviceToken = dto.DeviceToken;

            await _dbContext.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) throw new System.Exception("Người dùng không tồn tại.");

            user.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> SearchByNameAsync(string name)
        {
            return await _dbContext.Users
                .Where(u => u.Name.Contains(name) && !u.IsDeleted)
                .Select(u => new UserDTO
                {
                    Id = u.Id.ToString(),
                    Name = u.Name,
                    Email = u.Email,
                    IsDeleted = u.IsDeleted
                }).ToListAsync();
        }

        public async Task SoftDeleteMultipleAsync(List<int> ids)
        {
            var users = await _dbContext.Users.Where(u => ids.Contains(u.Id)).ToListAsync();
            foreach (var user in users)
            {
                user.IsDeleted = true;
            }

            await _dbContext.SaveChangesAsync();
        }
    }

    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task UpdateAsync(int id, UserDTO dto);
        Task SoftDeleteAsync(int id);
        Task CreateAsync(UserDTO dto);
        Task<List<UserDTO>> SearchByNameAsync(string name);
        Task SoftDeleteMultipleAsync(List<int> ids);
    }

    public class UserService : IUserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public Task CreateAsync(UserDTO dto)
        {
            var user = new User
            {
                Id = int.Parse(dto.Id),
                Email = dto.Email,
                Name = dto.Name,
                DeviceToken = dto.DeviceToken,
                IsDeleted = false,
                CreatedAt = System.DateTime.UtcNow
            };
            return _userRepo.CreateUserAsync(user, "hashed_password");
        }

        public Task<List<UserDTO>> GetAllAsync() => _userRepo.GetAllAsync();
        public Task<UserDTO> GetByIdAsync(int id) => _userRepo.GetByIdAsync(id);
        public Task UpdateAsync(int id, UserDTO dto) => _userRepo.UpdateAsync(id, dto);
        public Task SoftDeleteAsync(int id) => _userRepo.SoftDeleteAsync(id);
        public Task<List<UserDTO>> SearchByNameAsync(string name) => _userRepo.SearchByNameAsync(name);
        public Task SoftDeleteMultipleAsync(List<int> ids) => _userRepo.SoftDeleteMultipleAsync(ids);
    }
}
