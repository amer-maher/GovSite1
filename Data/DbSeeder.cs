using GovSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GovSite.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var ctx     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
        var config  = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        // 0) تأكد من القاعدة/المخطط
        await ctx.Database.MigrateAsync();

        // 1) الأدوار
        if (!await roleMgr.Roles.AnyAsync())
        {
            await roleMgr.CreateAsync(new AppRole { Name = "Admin" });
            await roleMgr.CreateAsync(new AppRole { Name = "Editor" });
        }

        // 2) القوالب (Templates)
        if (!await ctx.Templates.AnyAsync())
        {
            ctx.Templates.AddRange(
                new Template {
                    Name = "قالب كلاسيكي",
                    CssFile = "/templates/classic.css",
                    LayoutPath = "~/Views/Shared/Templates/_ClassicLayout.cshtml",
                    PreviewImageUrl = "/templates/classic.png"
                },
                new Template {
                    Name = "قالب حديث",
                    CssFile = "/templates/modern.css",
                    LayoutPath = "~/Views/Shared/Templates/_ModernLayout.cshtml",
                    PreviewImageUrl = "/templates/modern.png"
                }
            );
            await ctx.SaveChangesAsync();
        }

        // 3) جهة + بيانات تجريبية
        if (!await ctx.Agencies.AnyAsync())
        {
            // اربط الجهة بأول قالب
            var firstTemplateId = await ctx.Templates
                .OrderBy(t => t.Id).Select(t => t.Id).FirstAsync();

            var ag = new Agency { Name = "وزارة الصحة", Slug = "moh", ThemeKey = "default", TemplateId = firstTemplateId };
            ctx.Agencies.Add(ag);
            await ctx.SaveChangesAsync();

           ctx.Pages.Add(new Page {
    AgencyId = ag.Id,
    Title = "الرئيسية",
    Slug = "home",
    IsHome = true,
    HeroTitle = "مرحباً بكم في الموقع الرسمي",
    HeroSubtitle = "هذه الصفحة الرئيسية التجريبية",
    HeroBgColor = "#f8f9fa",
    HeroTextColor = "#000000",
    BlocksJson = "[]"
});


            ctx.Settings.AddRange(
                new Setting { AgencyId = ag.Id, Key = "PrimaryColor", Value = "#0d6efd" },
                new Setting { AgencyId = ag.Id, Key = "SecondaryColor", Value = "#198754" }
            );

            ctx.News.Add(new NewsPost
            {
                AgencyId = ag.Id,
                Title = "بيان صحفي",
                Slug = "press-1",
                Summary = "إطلاق المنصّة التجريبية.",
                HtmlContent = "<p>تم إطلاق المنصّة التجريبية للجهات الحكومية.</p>",
                IsPublished = true,
                PublishedAt = DateTime.UtcNow
            });

            await ctx.SaveChangesAsync();
        }

        // 4) مستخدم الأدمن
        var email    = config["Seed:AdminEmail"];
        var password = config["Seed:AdminPassword"];
        var phone    = config["Seed:AdminPhone"];

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new Exception("القيم Seed:AdminEmail و Seed:AdminPassword غير مهيّأة في appsettings.json");

        var admin = await userMgr.FindByEmailAsync(email);
        if (admin is null)
        {
            var firstAgencyId = await ctx.Agencies
                .OrderBy(a => a.Id).Select(a => a.Id).FirstOrDefaultAsync();

            admin = new AppUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phone,
                EmailConfirmed = true,
                AgencyId = firstAgencyId
            };

            var create = await userMgr.CreateAsync(admin, password);
            if (!create.Succeeded)
                throw new Exception("فشل إنشاء الأدمن: " +
                    string.Join(", ", create.Errors.Select(e => e.Description)));

            await userMgr.AddToRoleAsync(admin, "Admin");
        }
        else
        {
            // تأكد أنه ضمن دور Admin
            if (!await userMgr.IsInRoleAsync(admin, "Admin"))
                await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
