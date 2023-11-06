using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmsAmbulanceApp.Web.Client.Controllers
{
    [Authorize]
    public class AmbulanceRequest : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
