using BlazorSpark.Example.Application.Database;
using BlazorSpark.Example.Application.Models;
using BlazorSpark.Example.Application.Startup;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlazorSpark.Example.Application.Services.Auth
{
    public class UsersService
    {
        private readonly IDbContextFactory<DatabaseContext> _factory;
        private readonly AuthenticationStateProvider _stateProvider;

        public UsersService(IDbContextFactory<DatabaseContext> factory, AuthenticationStateProvider stateProvider)
        {
            _factory = factory;
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

        public async Task<User?> FindUserAsync(int userId)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FindAsync(userId);
        }

        public async Task<User?> FindUserAsync(string username, string password)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password);
		}

		public async Task<User?> FindUserByEmailAsync(string email)
		{
			using var context = _factory.CreateDbContext();
			return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
		}

		public async Task<User> CreateUserAsync(User user)
        {
            using var context = _factory.CreateDbContext();
            var addedUser = await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await context.UserRoles.AddAsync(new UserRole { RoleId = 1, User = user });
            await context.SaveChangesAsync();
            return addedUser.Entity;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Users
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
