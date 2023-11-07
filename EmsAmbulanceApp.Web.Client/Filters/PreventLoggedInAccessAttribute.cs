using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmsAmbulanceApp.Web.Client.Filters;

public class PreventLoggedInAccessAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Check if the user is already authenticated
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            // Redirect to a different page if the user is already logged in
            context.Result = new RedirectToActionResult("Index", "AmbulanceRequestController", null);
        }

        base.OnActionExecuting(context);
    }
}
