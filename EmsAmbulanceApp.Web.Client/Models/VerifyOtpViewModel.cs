namespace EmsAmbulanceApp.Web.Client.Models;

public class VerifyOtpViewModel
{
    public required string PhoneNumber { get; set; }
    public int? Otp1 { get; set; }
    public int? Otp2 { get; set; }
    public int? Otp3 { get; set; }
    public int? Otp4 { get; set; }
    public int? Otp5 { get; set; }
    public int? Otp6 { get; set; }
}
