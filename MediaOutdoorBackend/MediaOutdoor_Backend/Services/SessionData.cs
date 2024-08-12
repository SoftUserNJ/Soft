using MediaOutdoor_Backend.VMs;

namespace MediaOutdoor_Backend.Services
{
    public class SessionData
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public SessionData(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public PublicData GetData()
        {
            PublicData data = new PublicData();
            data.UserId = Convert.ToInt32(_session.GetString("UserId"));
            data.UserName = _session.GetString("UserName");
            data.Password = _session.GetString("Password");
            data.FirstName = _session.GetString("FirstName");
            data.SecondName = _session.GetString("SecondName");
            data.Gender = _session.GetString("Gender");
            data.UserType = _session.GetString("UserType");
            return data;
        }
    }
}
