using CityTechWEBAPI;
using CityTechWEBAPI.Models;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Authenticate(string username, string password)

    {
        var user = await _userRepository.GetUserByUsername(username, password);

        if (user == null)
        {
            return false;
        }

        return user.Password == password;
    }
}
