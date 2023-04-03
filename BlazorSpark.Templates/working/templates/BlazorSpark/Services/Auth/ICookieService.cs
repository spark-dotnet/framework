using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlazorSpark.Default.Services.Auth
{
	public interface ICookieService
	{
		Task ValidateAsync(CookieValidatePrincipalContext context);
	}
}
