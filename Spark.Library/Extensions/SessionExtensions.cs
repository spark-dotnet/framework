using Microsoft.AspNetCore.Http;

namespace Spark.Library.Extensions;

public static class SessionExtensions
{
    public static void SetFlash(this HttpContext context, string key, string value)
    {
        context.Session.SetString(key, value);
    }

    public static string? GetFlash(this HttpContext context, string key)
    {
        string? message = context.Session.GetString(key);
        context.Session.Remove(key);
        return message;
    }

    public static bool HasKey(this HttpContext context, string key)
    {
        string? message = context.Session.GetString(key);
        return string.IsNullOrEmpty(message) ? false : true;
    }
}
