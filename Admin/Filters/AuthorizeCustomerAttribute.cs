using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Admin.Models;

namespace Admin.Filters;

public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //var customerID = context.HttpContext.Session.GetString(nameof(AdminLogin.Username));
        if (string.IsNullOrEmpty(context.HttpContext.Session.GetString(nameof(AdminLogin.Username))))
            context.Result = new RedirectToActionResult("Index", "Home", null);
        
    }
}
