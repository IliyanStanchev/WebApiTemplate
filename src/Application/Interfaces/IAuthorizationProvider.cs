namespace WebApiTemplate.Application.Interfaces;
public interface IAuthorizationProvider
{
    Task<string> GenerateTokenAsync();
}
