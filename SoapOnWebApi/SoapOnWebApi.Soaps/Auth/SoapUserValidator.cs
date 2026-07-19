using SoapOnWebApi.Soaps.Interfaces;

namespace SoapOnWebApi.Soaps.Auth;

// Infrastructure/Auth/SoapUserValidator.cs
public class SoapUserValidator : ISoapUserValidator
{
    private const string _username = "@Username";
    private const string _password = "@Password";
    public SoapUserValidator()
    {
    }

    public async Task<bool> ValidateAsync(string username, string password)
    {
        await Task.CompletedTask;
        return username == _username && password == _password;
    }
}