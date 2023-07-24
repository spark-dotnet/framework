using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Library.Auth
{
    public interface IAuthValidator
    {
        Task ValidateAsync(CookieValidatePrincipalContext context);
    }
}
