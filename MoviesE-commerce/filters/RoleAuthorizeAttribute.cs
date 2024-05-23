using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoviesE_commerce.Models;
using MoviesE_commerce.Models.ViewModels;
using Newtonsoft.Json;

namespace MoviesE_commerce.filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly userRole _role;

        public RoleAuthorizeAttribute(userRole role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionObj = context.HttpContext.Session.GetString("LoginDetails");
            if(string.IsNullOrEmpty(sessionObj))
            {
                context.Result = new RedirectToActionResult("LogIn", "Register", null);
                return;
            }
            var loginDetails = JsonConvert.DeserializeObject<LoginViewModel>(sessionObj);
            if ((loginDetails.Role != _role))
            {
                context.Result = new ForbidResult();

            }
        }
    }
}
