using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorSpark.Example.Services.Auth
{
	public interface ICookieService
	{
		Task ValidateAsync(CookieValidatePrincipalContext context);
	}
}
