using GovSite.Data;
using GovSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseMySql(builder.Configuration.GetConnectionString("Default"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default"))));

// Identity
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie
builder.Services.ConfigureApplicationCookie(o =>
{
    o.Cookie.Name = ".GovSite.Auth";
    o.LoginPath = "/Account/Login";
    o.AccessDeniedPath = "/Account/AccessDenied";
    o.SlidingExpiration = true;
    o.ExpireTimeSpan = TimeSpan.FromHours(12);
});

// مهم
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// ✅ دعم attribute routing
app.MapControllers();

// مسارات تقليدية (لو محتاجها)
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// (اختياري) خلّي الجذر يروح للّوجين
// app.MapGet("/", () => Results.Redirect("/Account/Login"));

await DbSeeder.SeedAsync(app);
app.Run();
