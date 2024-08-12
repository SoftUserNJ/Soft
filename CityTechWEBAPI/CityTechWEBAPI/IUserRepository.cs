using CityTechWEBAPI.Models;


namespace CityTechWEBAPI
{
    public interface IUserRepository
    {
        Task<TblUser> GetUserByUsername(string username,string password);
    }

}
