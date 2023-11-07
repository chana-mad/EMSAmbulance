using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IServices;
using EmsAmbulanceApp.Web.Client.Domain.Values.SmsMessages;
using EmsAmbulanceApp.Web.Client.Filters;
using EmsAmbulanceApp.Web.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmsAmbulanceApp.Web.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Domain.Entities.Client> _userManager;
        private readonly SignInManager<Domain.Entities.Client> _signInManager;
        private readonly IOtpService _otpService;
        private readonly IEmsAmbulanceAppUnitOfWork _ecmUnitOfWork;
        public AccountController(SignInManager<Domain.Entities.Client> signInManager,
                                 UserManager<Domain.Entities.Client> userManager,
                                 IOtpService otpService,
                                 IEmsAmbulanceAppUnitOfWork ecmUnitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _otpService = otpService;
            _ecmUnitOfWork = ecmUnitOfWork;
        }

        [PreventLoggedInAccess]
        public IActionResult Index()
        {
            return View();
        }

        [PreventLoggedInAccess]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [PreventLoggedInAccess]
        public async Task<ActionResult> Register(RegisterViewModel sendOtpViewModel)
        {
            if (ModelState.IsValid)
            {
                var existUser = await _ecmUnitOfWork.Client.FindByPhoneNumberAsync(sendOtpViewModel.PhoneNumber);

                if (existUser != null)
                {
                    return RedirectToAction("VerifyOtp", "Authentication", new { phoneNumber = sendOtpViewModel.PhoneNumber });
                }

                var user = new Domain.Entities.Client
                {
                    UserName = sendOtpViewModel.PhoneNumber,
                    PhoneNumber = sendOtpViewModel.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    var otp = await _otpService.GenerateOtpAsync();
                    await _otpService.StoreOtpAsync(user.Id, sendOtpViewModel.PhoneNumber, otp);
                    await _otpService.SendSmsAsync(sendOtpViewModel.PhoneNumber, string.Format(SmsMessageBodies.otpMessage, otp));

                    return RedirectToAction("VerifyOtp", new { phoneNumber = sendOtpViewModel.PhoneNumber });
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                
                return View(sendOtpViewModel);
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
        [ValidateAntiForgeryToken]
        [PreventLoggedInAccess]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var otpVal = model.Otp1.ToString() +
                    model.Otp2.ToString() +
                    model.Otp3.ToString() +
                    model.Otp4.ToString() +
                    model.Otp5.ToString() +
                    model.Otp6.ToString();

                var otp = await _ecmUnitOfWork.OtpEntry
                .FindAsync(x => x.PhoneNumber == model.PhoneNumber && x.Otp == otpVal);

                if (otp != null && _otpService.VerifyOtp(otp))
                {
                    var user = await _userManager.FindByIdAsync(otp.ClientId);
                    if (user != null)
                    {
                        user.PhoneNumberConfirmed = true;
                        await _userManager.UpdateAsync(user);

                        // Sign in the user
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "AmbulanceRequestController");
                    }
                    ModelState.AddModelError(string.Empty, "Unable to find a user for the provided ID.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP.");
                }
            }

            // Something failed, redisplay the form
            return View(model);
        }
    }
}
