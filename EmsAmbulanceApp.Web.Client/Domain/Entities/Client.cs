using Microsoft.AspNetCore.Identity;

namespace EmsAmbulanceApp.Web.Client.Domain.Entities;

public class Client : IdentityUser
{
    public string? FullName { get; set; }
}
