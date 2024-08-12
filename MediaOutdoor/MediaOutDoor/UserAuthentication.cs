
using MediaOutdoor.Models;
using MediaOutDoor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MediaOutDoor
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

            string? userId = context.HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Index", "MOD", null);
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

            //if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(user.Role))
            //{
            //    context.Result = new ForbidResult();
            //    return;
            //}

            TblCustomer user = _context.TblCustomers.Where(x => x.CustomerId.Equals(Convert.ToInt32(userId))).FirstOrDefault();

            if (user == null)
            {
                context.Result = new RedirectToActionResult("SignIn", "MOD", null);
                return;
            }


        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            string? UserName = context.HttpContext.Session.GetString("UserName");
            string? Password = context.HttpContext.Session.GetString("Pas");

            var db = context.HttpContext.RequestServices.GetRequiredService<MediaOutdoorContext>
                ();

            if (!string.IsNullOrEmpty(UserName))
            {
                if (!string.IsNullOrEmpty(Password))
                {

                    TblCustomer? user = db.TblCustomers.Where(x => (x.UserName).ToLower().Equals(UserName.ToLower()) && x.Password.Equals(Password)).FirstOrDefault();
                    if (user == null)
                    {
                        //context.Result = new RedirectResult("Index");
                        context.Result = new RedirectToActionResult("Index", "MOD", null);

                    }
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "MOD", null);

                    //context.Result = new RedirectResult("Index");
                }

            }
            else
            {
                //context.Result = new RedirectResult("Index");
                context.Result = new RedirectToActionResult("Index", "MOD", null);

            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
