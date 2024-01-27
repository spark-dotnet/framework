using Spark.Templates.Razor.Application.Database;
using Spark.Templates.Razor.Application.Models;
using Spark.Templates.Razor.Application.Startup;
using Microsoft.EntityFrameworkCore;

namespace Spark.Templates.Razor.Application.Services.Auth;

public class RolesService
{
    private readonly DatabaseContext _db;

    public RolesService(DatabaseContext db)
    {
        _db = db;
    }

    public async Task<List<Role>> FindUserRolesAsync(int userId)
    {
        var roles = await _db.Roles.Where(role => role.UserRoles.Any(x => x.UserId == userId)).ToListAsync();
        return roles;
    }

    public async Task<bool> IsUserInRole(int userId, string roleName)
    {
        var userRolesQuery = from role in _db.Roles
                             where role.Name == roleName
                             from user in role.UserRoles
                             where user.UserId == userId
                             select role;
        var userRole = await userRolesQuery.FirstOrDefaultAsync();
        return userRole != null;
    }

    public async Task<List<User>> FindUsersInRoleAsync(string roleName)
    {
        var roleUserIdsQuery = from role in _db.Roles
                               where role.Name == roleName
                               from user in role.UserRoles
                               select user.UserId;
        return await _db.Users.Where(user => roleUserIdsQuery.Contains(user.Id))
            .ToListAsync();
    }
}
