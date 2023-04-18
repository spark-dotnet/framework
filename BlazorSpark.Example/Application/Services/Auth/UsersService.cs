using BlazorSpark.Example.Application.Database;
using BlazorSpark.Example.Application.Events;
using BlazorSpark.Example.Application.Models;
using Coravel.Events.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BlazorSpark.Example.Application.Services.Auth
{
    public class UsersService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UsersService(IDbContextFactory<ApplicationDbContext> factory, AuthenticationStateProvider authenticationStateProvider)
        {
            _factory = factory;
            _authenticationStateProvider = authenticationStateProvider ??
                                           throw new ArgumentNullException(nameof(authenticationStateProvider));
        }

        public async Task<string?> GetUserIdAsync()
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Identity?.Name;
        }

        public async Task<string?> GetUserClaimValueAsync(string claimType)
        {
            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Claims.FirstOrDefault(x => string.Equals(x.Type, claimType, StringComparison.Ordinal))?.Value;
        }

        public async Task<User> FindUserAsync(int userId)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FindAsync(userId);
        }

        public async Task<User> FindUserAsync(string username, string password)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email == username && x.Password == password);
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
