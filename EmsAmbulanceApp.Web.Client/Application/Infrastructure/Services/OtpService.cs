using EmsAmbulanceApp.Web.Client.Domain.Entities;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IRepositories;
using EmsAmbulanceApp.Web.Client.Domain.Interfaces.IServices;
using static System.Net.WebRequestMethods;

namespace EmsAmbulanceApp.Web.Client.Application.Infrastructure.Services;

public class OtpService : IOtpService
{
    private readonly Random _random = new();
    private readonly IEmsAmbulanceAppUnitOfWork _unitOfWork;

    public OtpService(IEmsAmbulanceAppUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GenerateOtpAsync()
    {
        string otp = string.Empty;

        await Task.Run(() => 
        { 
            otp = _random.Next(0, 999999).ToString("D6");
        });

        return otp;
    }

    public Task SendSmsAsync(string phoneNumber, string message)
    {
        return Task.FromResult(0);
    }

    public async Task StoreOtpAsync(string clientId ,string phoneNumber, string otp)
    {
        var otpEntry = new OtpEntry
        {
            ClientId = clientId,
            PhoneNumber = phoneNumber,
            Otp = otp,
            ExpiryTime = DateTime.UtcNow.AddMinutes(5),
        };
        await _unitOfWork.OtpEntry.AddAsync(otpEntry);
        await _unitOfWork.Complete();
    }

    public bool VerifyOtp(OtpEntry otpEntry)
    {
        if (otpEntry == null)
            throw new ArgumentNullException(nameof(otpEntry));

       return (otpEntry != null && otpEntry.ExpiryTime > DateTime.UtcNow);
    }
}
