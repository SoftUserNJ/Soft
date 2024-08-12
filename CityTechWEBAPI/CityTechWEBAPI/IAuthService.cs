namespace CityTechWEBAPI
{
public interface IAuthService
{
    Task<bool> Authenticate(string username, string password);
}

}
