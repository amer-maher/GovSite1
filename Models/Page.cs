using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GovSite.Models
{
  public class Page
  {
    public int Id { get; set; }

    public int AgencyId { get; set; }

    [ValidateNever]            // لا تتحقق منها ولا تربطها من الفورم
    public Agency? Agency { get; set; }   // خليها nullable

    public string Title { get; set; } = default!;
    public string Slug  { get; set; } = default!;
    public bool IsHome  { get; set; }

    public string? ThemePrimary     { get; set; }
    public string? BackgroundColor  { get; set; }

    // Hero
    public string? HeroTitle     { get; set; }
    public string? HeroSubtitle  { get; set; }
    public string? HeroImageUrl  { get; set; }
    public string? HeroBgColor   { get; set; }
    public string? HeroTextColor { get; set; }

    // بلوكات ديناميكية محفوظة كـ JSON
    public string? BlocksJson { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
  }
}
