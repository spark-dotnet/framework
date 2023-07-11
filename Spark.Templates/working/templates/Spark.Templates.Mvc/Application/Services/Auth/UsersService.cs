using Spark.Templates.Mvc.Application.Database;
using Spark.Templates.Mvc.Application.Models;
using Spark.Templates.Mvc.Application.Startup;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Spark.Templates.Mvc.Application.Services.Auth
{
    public class UsersService
    {
        private readonly DatabaseContext _db;
        private readonly AuthenticationStateProvider _stateProvider;

        public UsersService(DatabaseContext db, AuthenticationStateProvider stateProvider)
        {
            _db = db;
            _stateProvider = stateProvider;
        }

        public async Task<User?> GetAuthenticatedUser()
        {
            var userId = await this.GetAuthenticatedUserId();
            if (userId != null)
            {
                var id = userId ?? default(int);
                return await this.FindUserAsync(id);
            }
            return null;
        }

        public async Task<int?> GetAuthenticatedUserId()
        {
            var authState = await _stateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var test = user.Claims;
                var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out var userId))
                {
                    return null;
                }
                return userId;
            }
            return null;
        }

        public async Task<User> FindUserAsync(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }

        public async Task<User?> FindUserAsync(string username, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password);
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var addedUser = await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            await _db.UserRoles.AddAsync(new UserRole { RoleId = 1, User = user });
            await _db.SaveChangesAsync();
            return addedUser.Entity;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _db.Users
                    .ToListAsync();
        }

        public string GetSha256Hash(string input)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var byteValue = Encoding.UTF8.GetBytes(input);
                var byteHash = hashAlgorithm.ComputeHash(byteValue);
                return Convert.ToBase64String(byteHash);
            }
        }
    }
}
