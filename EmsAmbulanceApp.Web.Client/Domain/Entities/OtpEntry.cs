namespace EmsAmbulanceApp.Web.Client.Domain.Entities;

public class OtpEntry
{
    public int Id { get; set; }
    public required string ClientId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Otp { get; set; }
    public DateTime ExpiryTime { get; set; }
}
