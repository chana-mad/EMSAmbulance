using EmsAmbulanceApp.Web.Client.Domain.Entities;

namespace EmsAmbulanceApp.Web.Client.Domain.Interfaces.IServices;

public interface IOtpService
{
    Task<string> GenerateOtpAsync();
    bool VerifyOtp(OtpEntry otpEntry);
    Task SendSmsAsync(string phoneNumber, string message);
    Task StoreOtpAsync(string clientId, string phoneNumber, string otp);
}
