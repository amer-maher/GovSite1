using System;
using System.Linq;
using System.Threading.Tasks;
using GovSite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// علشان ما نصطدم بالاسم Page تبع الموديل
using PageModel = GovSite.Models.Page;

namespace GovSite.Controllers
{
    // الأساس: /Pages
    [Route("Pages")]
    [Authorize] // افتح فقط للإدارة. هنخلّي PublicLink عام بالأسفل.
    public class PagesController : Controller
    {
        private readonly AppDbContext _db;
        public PagesController(AppDbContext db) => _db = db;

        // =============== Helpers ===============

        // توليد/تنظيف Slug بسيط
        private static string NormalizeSlug(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            var slug = s.Trim().ToLowerInvariant();
            slug = slug.Replace(" ", "-").Replace("_", "-");
            // إزالة أحرف غير مقبولة
            slug = new string(slug.Where(ch => char.IsLetterOrDigit(ch) || ch == '-').ToArray());
            // إزالة تكرار '-'
            while (slug.Contains("--")) slug = slug.Replace("--", "-");
            return slug.Trim('-');
        }

        private async Task<bool> SlugExistsAsync(int agencyId, string slug, int? excludeId = null)
        {
            var q = _db.Pages.AsNoTracking()
                     .Where(p => p.AgencyId == agencyId && p.Slug == slug);
            if (excludeId.HasValue) q = q.Where(p => p.Id != excludeId.Value);
            return await q.AnyAsync();
        }

        // =============== CRUD ===============

        // GET /Pages
        [HttpGet("")]
        public async Task<IActionResult> Index(int? agencyId, string? q)
        {
            var query = _db.Pages.Include(p => p.Agency).AsNoTracking().AsQueryable();

            if (agencyId.HasValue) query = query.Where(p => p.AgencyId == agencyId.Value);
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => p.Title.Contains(q) || p.Slug.Contains(q));

            var list = await query.OrderByDescending(p => p.UpdatedAt).ToListAsync();
            return View(list); // Views/Pages/Index.cshtml
        }

        // GET /Pages/Create  (اختياري تمرير agencyId لتعبئته)
        [HttpGet("Create/{agencyId:int?}")]
        public IActionResult Create(int? agencyId = null)
        {
            var vm = new PageModel
            {
                AgencyId = agencyId ?? 0,
                HeroBgColor = "#ffffff",
                HeroTextColor = "#000000",
                BackgroundColor = "#ffffff"
            };
            return View(vm); // Views/Pages/Create.cshtml
        }

        // POST /Pages/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageModel vm)
        {
            if (vm == null) return BadRequest();

            vm.Slug = NormalizeSlug(vm.Slug);
            if (!vm.IsHome && string.IsNullOrWhiteSpace(vm.Slug))
                ModelState.AddModelError(nameof(vm.Slug), "الـ Slug مطلوب عند عدم كون الصفحة رئيسية.");

            if (await SlugExistsAsync(vm.AgencyId, vm.Slug))
                ModelState.AddModelError(nameof(vm.Slug), "هذا الـ Slug مستخدم مسبقًا داخل نفس الجهة.");

            if (!ModelState.IsValid) return View(vm);

            vm.UpdatedAt = DateTime.UtcNow;

            _db.Pages.Add(vm);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { agencyId = vm.AgencyId });
        }

        [HttpGet("Edit/{id:int}")]
public async Task<IActionResult> Edit(int id)
{
    var page = await _db.Pages
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

    if (page == null) return NotFound();
    return View(page);
}

[HttpPost("Edit/{id:int}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, GovSite.Models.Page vm)
{
    if (id != vm.Id)
    {
        ModelState.AddModelError("", "عدم تطابق المعرّف (ID).");
        return View(vm);
    }

    // مثال بسيط لتنظيف الـ Slug (اختياري)
    string NormalizeSlug(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "";
        var slug = s.Trim().ToLowerInvariant()
                    .Replace(" ", "-").Replace("_", "-");
        slug = new string(slug.Where(ch => char.IsLetterOrDigit(ch) || ch == '-').ToArray());
        while (slug.Contains("--")) slug = slug.Replace("--", "-");
        return slug.Trim('-');
    }
    vm.Slug = NormalizeSlug(vm.Slug);

    // لو الصفحة ليست Home يجب أن يكون slug غير فارغ
    if (!vm.IsHome && string.IsNullOrWhiteSpace(vm.Slug))
        ModelState.AddModelError(nameof(vm.Slug), "الـ Slug مطلوب للصفحات غير الرئيسية.");

    // أظهر كل أخطاء الربط/التحقق أعلى الفورم
    foreach (var e in ModelState.Where(kv => kv.Value?.Errors.Count > 0))
        ModelState.AddModelError("", $"{e.Key}: {string.Join(", ", e.Value!.Errors.Select(x => x.ErrorMessage))}");

    if (!ModelState.IsValid) return View(vm);

    var page = await _db.Pages.FirstOrDefaultAsync(p => p.Id == id);
    if (page == null) return NotFound();

    // ⚠️ لا تغيّر AgencyId هنا (حتى لا تنسخه 0 من الحقل المخفي)
    // page.AgencyId = vm.AgencyId;

    // حدّث الحقول
    page.Title           = vm.Title;
    page.Slug            = vm.Slug;
    page.IsHome          = vm.IsHome;

    page.ThemePrimary    = vm.ThemePrimary;
    page.BackgroundColor = vm.BackgroundColor;

    page.HeroTitle       = vm.HeroTitle;
    page.HeroSubtitle    = vm.HeroSubtitle;
    page.HeroImageUrl    = vm.HeroImageUrl;
    page.HeroBgColor     = vm.HeroBgColor;
    page.HeroTextColor   = vm.HeroTextColor;

    page.BlocksJson      = vm.BlocksJson;
    page.UpdatedAt       = DateTime.UtcNow;

    await _db.SaveChangesAsync();

    TempData["ok"] = "تم الحفظ بنجاح ✅";
    // ارجع لنفس صفحة التعديل حتى تشوف القيم بعد الحفظ فورًا
    return RedirectToAction(nameof(Edit), new { id });
}

        // POST /Pages/Delete/1
        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var page = await _db.Pages.FindAsync(id);
            if (page == null) return NotFound();

            _db.Pages.Remove(page);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { agencyId = page.AgencyId });
        }

        // (اختياري) GET /Pages/Details/1
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var page = await _db.Pages
                .Include(p => p.Agency)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (page == null) return NotFound();
            return View(page); // Views/Pages/Details.cshtml (لو عملته)
        }

        // =============== Preview & PublicLink ===============

        // GET /Pages/Preview/1  — معاينة داخل لوحة الإدارة
        [HttpGet("Preview/{id:int}")]
        public async Task<IActionResult> Preview(int id)
        {
            var page = await _db.Pages
                .Include(p => p.Agency)
                    .ThenInclude(a => a.Template)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (page == null || page.Agency == null) return NotFound();

            // نفس البيانات التي يعتمدها الـ Layout العام
            ViewBag.AgencyName = page.Agency.Name;
            ViewBag.AgencySlug = page.Agency.Slug;
            ViewBag.AgencyLogo = page.Agency.LogoUrl;
            ViewBag.LayoutPath = page.Agency.Template?.LayoutPath
                                 ?? "~/Views/Shared/Templates/_ClassicLayout.cshtml";

            return View("DynamicPage", page); // استخدم نفس فيو العرض العام
        }

        // GET /Pages/PublicLink/1  — يبني رابط عام ثم يوجّه إليه
        [HttpGet("PublicLink/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> PublicLink(int id)
        {
            var page = await _db.Pages
                .Include(p => p.Agency)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (page == null || page.Agency == null) return NotFound();

            var path = "/" + page.Agency.Slug + "/" + (page.IsHome ? "" : page.Slug);
            path = path.Replace("//", "/");

            return Redirect(path); // سيصل إلى SiteController.Page (الكاش-أول)
        }
    }
}
