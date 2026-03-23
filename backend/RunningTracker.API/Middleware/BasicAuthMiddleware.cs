using System.Net.Http.Headers;
using System.Text;

namespace RunningTracker.API.Middleware;

public class BasicAuthMiddleware(RequestDelegate next, IConfiguration config)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            Challenge(context);
            return;
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(header!);
            if (authHeader.Scheme != "Basic" || authHeader.Parameter is null)
            {
                Challenge(context);
                return;
            }

            var credentials = Encoding.UTF8
                .GetString(Convert.FromBase64String(authHeader.Parameter))
                .Split(':', 2);

            var expectedUser = config["AUTH_USER"];
            var expectedPass = config["AUTH_PASSWORD"];

            if (credentials.Length != 2
                || credentials[0] != expectedUser
                || credentials[1] != expectedPass)
            {
                Challenge(context);
                return;
            }
        }
        catch
        {
            Challenge(context);
            return;
        }

        await next(context);
    }

    private static void Challenge(HttpContext context)
    {
        context.Response.StatusCode = 401;
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Road to 10K\"";
    }
}
