namespace MiniCommerce.Api.App.Auth.Signatures;

public interface IBasicAuthenticationService
{
    bool ValidateUser(string username, string password);
}