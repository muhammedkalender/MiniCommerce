using MiniCommerce.Api.App.Auth.Signatures;

namespace MiniCommerce.Api.App.Auth.Services;

public class BasicAuthenticationService : IBasicAuthenticationService
{
    public bool ValidateUser(string username, string password)
    {
        // Just for mocking no need real credentials, this project places as like micro service 
        return username == "admin" && password == "1234";
    }
}