using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IServices;
using EmsAmbulanceApp.Web.Client.Domain.Values.SmsMessages;
using EmsAmbulanceApp.Web.Client.Filters;
using EmsAmbulanceApp.Web.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmsAmbulanceApp.Web.Client.Controllers;

public class AuthenticationController : Controller
{
    private readonly IOtpService _otpService;
    private readonly IEmsAmbulanceAppUnitOfWork _ecmUnitOfWork;
    private readonly SignInManager<Domain.Entities.Client> _signInManager;

    public AuthenticationController(IOtpService otpService,
                                    IEmsAmbulanceAppUnitOfWork ecmUnitOfWork,
                                    SignInManager<Domain.Entities.Client> signInManager)
    {
        _otpService = otpService;
        _ecmUnitOfWork = ecmUnitOfWork;
        _signInManager = signInManager;
    }

    [PreventLoggedInAccess]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [PreventLoggedInAccess]
    public async Task<ActionResult> SendOtp(SendOtpViewModel sendOtpViewModel)
    {
        if (ModelState.IsValid)
        {
            var otp = await _otpService.GenerateOtpAsync();
            await _otpService.StoreOtpAsync("", sendOtpViewModel.PhoneNumber, otp);
            await _otpService.SendSmsAsync(sendOtpViewModel.PhoneNumber, string.Format(SmsMessageBodies.otpMessage, otp));

            return RedirectToAction("VerifyOtp", new { phoneNumber = sendOtpViewModel.PhoneNumber });
        }

        return View(sendOtpViewModel);
    }

    [PreventLoggedInAccess]
    public ActionResult VerifyOtp(string phoneNumber)
    {
        var verify = new VerifyOtpViewModel
        {
            PhoneNumber = phoneNumber
        };

        return View(verify);
    }

    [HttpPost]
    [PreventLoggedInAccess]
    public async Task<ActionResult> VerifyOtp(VerifyOtpViewModel verifyOtpViewModel)
    {
        var user = await _ecmUnitOfWork.Client.FindByPhoneNumberAsync(verifyOtpViewModel.PhoneNumber);

        if (user != null && ModelState.IsValid)
        {
            var otpVal = verifyOtpViewModel.Otp1.ToString() +
                verifyOtpViewModel.Otp2.ToString() +
                verifyOtpViewModel.Otp3.ToString() +
                verifyOtpViewModel.Otp4.ToString() +
                verifyOtpViewModel.Otp5.ToString() +
                verifyOtpViewModel.Otp6.ToString();

            var otp = await _ecmUnitOfWork.OtpEntry
                .FindAsync(x => x.PhoneNumber == verifyOtpViewModel.PhoneNumber && x.Otp == otpVal);

            if (otp != null && _otpService.VerifyOtp(otp))
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                // await _ecmUnitOfWork.OtpEntry.RemoveAsync(otp);
                return RedirectToAction("Index", "AmbulanceRequest");
            }
            
            ModelState.AddModelError("", "The OTP is invalid or has expired.");

            return View(verifyOtpViewModel);
        }

        return View(verifyOtpViewModel);
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        // Sign the user out
        await _signInManager.SignOutAsync();
        // Redirect to homepage or login page after logging out
        return RedirectToAction("Index", "Account");
    }
}
