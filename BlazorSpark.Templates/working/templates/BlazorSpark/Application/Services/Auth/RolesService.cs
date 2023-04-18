using BlazorSpark.Default.Application.Database;
using BlazorSpark.Default.Application.Models;
using BlazorSpark.Default.Application.Startup; 
using Microsoft.EntityFrameworkCore;

namespace BlazorSpark.Default.Application.Services.Auth
{
    public class RolesService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _factory;

        public RolesService(IDbContextFactory<ApplicationDbContext> factory)
        {
            _factory = factory;
        }

        public async Task<List<Role>> FindUserRolesAsync(int userId)
        {
            using var context = _factory.CreateDbContext();
            var roles = await context.Roles.Where(role => role.UserRoles.Any(x => x.UserId == userId)).ToListAsync();
            return roles;
        }

        public async Task<bool> IsUserInRole(int userId, string roleName)
        {
            using var context = _factory.CreateDbContext();
            var userRolesQuery = from role in context.Roles
                                 where role.Name == roleName
                                 from user in role.UserRoles
                                 where user.UserId == userId
                                 select role;
            var userRole = await userRolesQuery.FirstOrDefaultAsync();
            return userRole != null;
        }

        public async Task<List<User>> FindUsersInRoleAsync(string roleName)
        {
            using var context = _factory.CreateDbContext();
            var roleUserIdsQuery = from role in context.Roles
                                   where role.Name == roleName
                                   from user in role.UserRoles
                                   select user.UserId;
            return await context.Users.Where(user => roleUserIdsQuery.Contains(user.Id))
                .ToListAsync();
        }
    }
}
