using GovSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GovSite.Controllers {
  [Authorize]
  [Route("admin")]                 // يربط /admin بالكنترولر
  public class AdminController : Controller {
    private readonly AppDbContext _db;
    public AdminController(AppDbContext db) => _db = db;

    [HttpGet("")]                  // GET /admin
    public async Task<IActionResult> Index() {
      var stats = new {
        Agencies = await _db.Agencies.CountAsync(),
        Pages    = await _db.Pages.CountAsync(),
        News     = await _db.News.CountAsync(),
        Media    = await _db.Media.CountAsync()
      };
      return View(stats);
    }
  }
}
