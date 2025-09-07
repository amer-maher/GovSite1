using GovSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AdminMaintenanceController : Controller
{
    private readonly UserManager<AppUser> _users;
    private readonly IConfiguration _cfg;
    private readonly IWebHostEnvironment _env;
    public AdminMaintenanceController(UserManager<AppUser> users, IConfiguration cfg, IWebHostEnvironment env)
    { _users = users; _cfg = cfg; _env = env; }

    // GET /AdminMaintenance/ResetAdminPassword
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ResetAdminPassword()
    {
        if (!_env.IsDevelopment())
            return Forbid(); // لا تتركه مفتوحاً في الإنتاج

        var email = _cfg["Seed:AdminEmail"]!;
        var newPass = _cfg["Seed:AdminPassword"]!;
        var user = await _users.FindByEmailAsync(email);
        if (user == null) return Content("Admin user not found.");

        // احذف أي باسورد سابق ثم عيّن الجديد
        var hasPassword = await _users.HasPasswordAsync(user);
        if (hasPassword)
        {
            var rem = await _users.RemovePasswordAsync(user);
            if (!rem.Succeeded) return Content("Failed to remove old password: " +
                string.Join(", ", rem.Errors.Select(e => e.Description)));
        }
        var add = await _users.AddPasswordAsync(user, newPass);
        if (!add.Succeeded) return Content("Failed to set new password: " +
            string.Join(", ", add.Errors.Select(e => e.Description)));

        return Content("Admin password has been reset.");
    }
}
