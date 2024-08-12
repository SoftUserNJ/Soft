using CityTech.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CityTech
{
    public class AllowPage : Attribute, IActionFilter
    {
        private readonly string[] _allowedRoles;

        public AllowPage(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string? userId = context.HttpContext.Session.GetString("UserId");
            var db = context.HttpContext.RequestServices.GetRequiredService<CityTechContext>();

            if (!string.IsNullOrEmpty(userId))
            {
                string? url = context.HttpContext.Request.Path;

                var UserN = (from U in db.TblUsers
                             join T in db.TblUserTypes on U.UserTypeId equals T.UserTypeId
                             where (U.UserId.Equals(Convert.ToInt32(userId)))
                             select new {name = U.UserName, type = T.UserType, }).FirstOrDefault();

                if (UserN.type.ToLower() != "admin")
                {
                    var page = db.TblSecurities.Where(x => x.UserId == Convert.ToInt32(userId) && x.Url.ToLower() == url.ToLower()).FirstOrDefault();

                    if (page == null)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
                    }
                }
            }
            else
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "Login" }
                });
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
