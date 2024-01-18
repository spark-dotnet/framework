using Microsoft.AspNetCore.Authentication.Cookies;

namespace Spark.Library.Auth;

public interface IAuthValidator
{
    Task ValidateAsync(CookieValidatePrincipalContext context);
}
