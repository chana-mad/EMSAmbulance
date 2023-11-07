using EmsAmbulanceApp.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmsAmbulanceApp.Web.Client.Controllers;

[Authorize]
public class AmbulanceRequestController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AmbulanceRequest([FromBody] AmbulanceRequestViewModel model)
    {
        // Your logic here
        return Json(new { success = true, message = "Data received successfully!" });
    }

}
