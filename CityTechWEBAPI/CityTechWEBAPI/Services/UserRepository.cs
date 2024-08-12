using System.Threading.Tasks;
using CityTechWEBAPI;
using CityTechWEBAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly CityTechDevContext _dbContext;
    public UserRepository(CityTechDevContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<TblUser> GetUserByUsername(string username, string  password)
    {
    
        return _dbContext.TblUsers.FirstOrDefaultAsync(u => u.UserName == username && u.Password== password);
    }

	
}
