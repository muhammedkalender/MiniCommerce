using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using MiniCommerce.Api.App.Auth.Signatures;

namespace MiniCommerce.Api.App.Auth.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IBasicAuthenticationService _authenticationService;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IBasicAuthenticationService authenticationService)
        : base(options, logger, encoder, clock)
    {
        _authenticationService = authenticationService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Path.StartsWithSegments("/swagger"))
        {
            return AuthenticateResult.NoResult();
        }

        if (Request.Headers.TryGetValue(HeaderNames.Authorization, out var auth) || auth.Count != 0)
        {
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(auth);
                if (authHeader.Scheme != "Basic")
                    return AuthenticateResult.Fail("Only Basic authentication supported");

                var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                if (!_authenticationService.ValidateUser(username, password))
                    return AuthenticateResult.Fail("Invalid credentials");

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }
        }
        else
        {
            return AuthenticateResult.Fail("Authorization header missing");
        }
    }
}