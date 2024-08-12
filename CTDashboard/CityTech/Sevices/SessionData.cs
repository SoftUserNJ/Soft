using CityTech.Models;
using CityTech.Models.ViewModel;

namespace CityTech.Sevices
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
            //data.Password = _session.GetString("Password");
            data.UserType = _session.GetString("UserType");
            data.UserTypeId = Convert.ToInt32(_session.GetString("UserTypeId"));
            data.UserSkill = _session.GetString("UserSkill");
            data.UserSkillId = Convert.ToInt32(_session.GetString("UserSkillId"));
            data.FcmToken = _session.GetString("FcmToken");
            return data;
        }
    }
}
