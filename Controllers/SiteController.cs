using GovSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GovSite.Controllers
{
    public class SiteController : Controller
    {
        private readonly AppDbContext _db;
        public SiteController(AppDbContext db) => _db = db;

[HttpGet("{agencySlug:regex(^(?!Pages$|pages$|Admin$|admin$|Account$|account$).*)}/{**slug}")]
[AllowAnonymous]
public async Task<IActionResult> Page(string agencySlug, string? slug)
        {
            var agency = await _db.Agencies
                .Include(a => a.Template)
                .FirstOrDefaultAsync(a => a.Slug == agencySlug);
            if (agency == null) return NotFound();

            var page = await _db.Pages
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.AgencyId == agency.Id &&
                    (string.IsNullOrEmpty(slug) ? p.IsHome : p.Slug == slug));
            if (page == null) return NotFound();

            // تمرير بيانات للـ Layout
            ViewBag.AgencyName = agency.Name;
            ViewBag.AgencySlug = agency.Slug;
            ViewBag.AgencyLogo = agency.LogoUrl;

            // اختر Layout من القالب (ولو غير موجود استخدم الكلاسيكي)
            var layout = agency.Template?.LayoutPath ?? "~/Views/Shared/Templates/_ClassicLayout.cshtml";
            ViewBag.LayoutPath = layout;

            return View("DynamicPage", page);
        }
    }
}
