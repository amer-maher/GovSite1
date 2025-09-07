using Microsoft.AspNetCore.Identity;

namespace GovSite.Models {
  public class AppUser : IdentityUser {
    public int? AgencyId { get; set; }
    public Agency? Agency { get; set; }
  }
  public class AppRole : IdentityRole { }
}
