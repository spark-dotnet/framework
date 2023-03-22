using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorSpark.Lib.Auth
{
	public interface ICookieService
	{
		Task ValidateAsync(CookieValidatePrincipalContext context);
	}
}
