using Microsoft.AspNetCore.Mvc;

namespace GovSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
            => RedirectToAction("Login", "Account"); // يرسل مباشرة لصفحة تسجيل الدخول
    }
}
