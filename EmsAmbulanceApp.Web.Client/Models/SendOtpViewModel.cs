using System.ComponentModel.DataAnnotations;

namespace EmsAmbulanceApp.Web.Client.Models;

public class SendOtpViewModel
{
    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
}
