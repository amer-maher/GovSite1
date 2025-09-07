using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovSite.Controllers;

[Authorize] // Require authentication for uploads
public class UploadController : Controller
{
    private static readonly string[] AllowedExt = [".png", ".jpg", ".jpeg", ".webp", ".gif", ".svg"];

    [HttpPost("/upload")]
    [RequestSizeLimit(20_000_000)] // 20MB
    [ValidateAntiForgeryToken] // Require CSRF token
    public async Task<IActionResult> Upload(IFormFile file, [FromServices] IWebHostEnvironment env)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "لا يوجد ملف" });

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExt.Contains(ext))
            return BadRequest(new { error = "نوع الملف غير مسموح" });

        var uploads = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads");
        Directory.CreateDirectory(uploads);

        var fname = $"{Guid.NewGuid():N}{ext}";
        var path = Path.Combine(uploads, fname);

        await using (var fs = System.IO.File.Create(path))
            await file.CopyToAsync(fs);

        // المسار العام الذي ستستخدمه في <img src=...>
        var url = $"/uploads/{fname}";
        return Json(new { url });
    }
}
