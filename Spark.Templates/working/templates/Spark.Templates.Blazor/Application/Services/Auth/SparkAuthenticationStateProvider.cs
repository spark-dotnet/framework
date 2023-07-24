using Spark.Templates.Blazor.Application.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.Security.Claims;

namespace Spark.Templates.Blazor.Application.Services.Auth
{
    public class SparkAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SparkAuthenticationStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory) : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override TimeSpan RevalidationInterval { get; } = TimeSpan.FromMinutes(2);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            // Get the user from a new scope to ensure it fetches fresh data
            var scope = _scopeFactory.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UsersService>();
                return await ValidateUserAsync(userManager, authenticationState?.User);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }

        private async Task<bool> ValidateUserAsync(UsersService userManager, ClaimsPrincipal? principal)
        {
            if (principal is null)
            {
                return false;
            }

            var userIdString = principal.FindFirst(ClaimTypes.UserData)?.Value;
            if (!int.TryParse(userIdString, out var userId))
            {
                return false;
            }

            var user = await userManager.FindUserAsync(userId);
            return user is not null;
        }
    }
}
