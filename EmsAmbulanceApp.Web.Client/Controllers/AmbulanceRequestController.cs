using EmsAmbulanceApp.Web.Client.Domain.Entities;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using EmsAmbulanceApp.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmsAmbulanceApp.Web.Client.Controllers;

[Authorize]
public class AmbulanceRequestController : Controller
{
    private readonly IEmsAmbulanceAppUnitOfWork _emsUnitOfWork;
    private readonly UserManager<Domain.Entities.Client> _userManager;

    public AmbulanceRequestController(IEmsAmbulanceAppUnitOfWork emsUnitOfWork, UserManager<Domain.Entities.Client> userManager)
    {
        _emsUnitOfWork = emsUnitOfWork;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        AmbulanceRequestViewModel model;
        var existingRequest = await _emsUnitOfWork.AmbulanceRequest
            .FindAsync(x => x.Status != Domain.Enums.AmbulanceRequestStatus.Completed 
                         && x.Status != Domain.Enums.AmbulanceRequestStatus.Denied
                         && x.Status != Domain.Enums.AmbulanceRequestStatus.Cancelled);

        if (existingRequest == null)
            model = new AmbulanceRequestViewModel();
        else
            model = new AmbulanceRequestViewModel
            {
                AmbulanceRequestId = existingRequest.Id
            };

        return View(model);
    }

    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> AmbulanceRequest(string phoneNumber, string pickupLocation)
    {
        var user = await _emsUnitOfWork.Client.FindByPhoneNumberAsync(phoneNumber);

        if (user == null)
            return NotFound();

        var ambulance = new AmbulanceRequest
        {
            ClientId = user.Id,
            PickupLocation = pickupLocation,
            Destination = "",
            Status = Domain.Enums.AmbulanceRequestStatus.Pending,
            RequestTime = DateTime.UtcNow
        };
        var ambulaceRequest = await _emsUnitOfWork.AmbulanceRequest.AddAsync(ambulance);
        await _emsUnitOfWork.Complete();
        return Json(ambulaceRequest);
    }

}
