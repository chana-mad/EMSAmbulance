using EmsAmbulanceApp.Web.Client.Domain.Enums;

namespace EmsAmbulanceApp.Web.Client.Domain.Entities;

public class AmbulanceRequest
{
    public int Id { get; set; }
    public required string ClientId { get; set; }
    public DateTime RequestTime { get; set; }
    public string? PickupLocation { get; set; }
    public string? Destination { get; set; }
    public AmbulanceRequestStatus Status { get; set; }
}
