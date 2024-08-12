using MediaOutdoor.VMs;

namespace MediaOutDoor.Sevices
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
            data.FirstName = _session.GetString("FirstName");
            data.SecondName = _session.GetString("SecondName");
            data.Email = _session.GetString("Email");
            data.Type = _session.GetString("Type");
            return data;
        }
    }
}
