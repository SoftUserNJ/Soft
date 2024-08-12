using MediaOutdoor_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MediaOutdoor_Backend
{
    public class UserAuthentication : Attribute, IActionFilter, IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;

        public UserAuthentication(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //if (context.HttpContext.Session == null)
            //{
            //    context.Result = new RedirectResult("/User/Login");
            //}

            string? userId = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectResult("/Backend/Login");
                return;
            }

            var _context = context.HttpContext.RequestServices.GetRequiredService<MediaOutdoorContext>();

            //var user = (from U in _context.TblUsers
            //            join T in _context.TblUserTypes on U.UserTypeId equals T.UserTypeId
            //            where (U.UserId.Equals(Convert.ToInt32(userId)))
            //            select new
            //            {
            //                Id = U.UserId,
            //                Name = U.UserName,
            //                Role = T.UserType
            //            }).FirstOrDefault();


            TblUser user = _context.TblUsers.Where(x => x.UserId.Equals(Convert.ToInt32(userId))).FirstOrDefault();

            if (user == null)
            {
                context.Result = new RedirectResult("/Backend/Login");
                return;
            }

            //if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(user.Role))
            //{
            //    context.Result = new ForbidResult();
            //    return;
            //}
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {

            string? UserName = context.HttpContext.Session.GetString("UserName");
            string? Password = context.HttpContext.Session.GetString("Password");

            var db = context.HttpContext.RequestServices.GetRequiredService<MediaOutdoorContext>();

            if (!string.IsNullOrEmpty(UserName))
            {
                if (!string.IsNullOrEmpty(Password))
                {

                    TblUser? user = db.TblUsers.Where(x => (x.UserName).ToLower().Equals(UserName.ToLower()) && x.Password.Equals(Password)).FirstOrDefault();
                    if (user == null)
                    {
                        context.Result = new RedirectResult("Login");
                    }
                }
                else
                {
                    context.Result = new RedirectResult("Login");
                }

            }
            else
            {
                context.Result = new RedirectResult("Login");
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
