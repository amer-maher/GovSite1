using GovSite.Data;
using GovSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GovSite.Controllers
{
    [Authorize]
    public class AgenciesController : Controller
    {
        private readonly AppDbContext _db;
        public AgenciesController(AppDbContext db) => _db = db;

        // قائمة الجهات
        public async Task<IActionResult> Index()
        {
            var agencies = await _db.Agencies.Include(a => a.Template).ToListAsync();
            return View(agencies);
        }

        // إنشاء
        public async Task<IActionResult> Create()
        {
            ViewBag.Templates = await _db.Templates.ToListAsync();
            return View(new Agency());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agency model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Templates = await _db.Templates.ToListAsync();
                return View(model);
            }
            _db.Agencies.Add(model);
            await _db.SaveChangesAsync();
            TempData["ToastMsg"] = "تم إنشاء الجهة بنجاح";
            return RedirectToAction(nameof(Index));
        }

        // تعديل
        public async Task<IActionResult> Edit(int id)
        {
            var agency = await _db.Agencies.FindAsync(id);
            if (agency == null) return NotFound();

            ViewBag.Templates = await _db.Templates.ToListAsync();
            return View(agency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Agency model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Templates = await _db.Templates.ToListAsync();
                return View(model);
            }
            _db.Agencies.Update(model);
            await _db.SaveChangesAsync();
            TempData["ToastMsg"] = "تم حفظ التغييرات";
            return RedirectToAction(nameof(Index));
        }
    }
}
