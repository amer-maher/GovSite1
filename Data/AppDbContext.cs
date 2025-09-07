using GovSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GovSite.Data {
  public class AppDbContext : IdentityDbContext<AppUser, AppRole, string> {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Agency> Agencies => Set<Agency>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<NewsPost> News => Set<NewsPost>();
    public DbSet<MediaFile> Media => Set<MediaFile>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Template> Templates => Set<Template>();

    protected override void OnModelCreating(ModelBuilder b) {
      base.OnModelCreating(b);
      b.Entity<Agency>().HasIndex(a => a.Slug).IsUnique();
      b.Entity<Page>().HasIndex(p => new { p.AgencyId, p.Slug }).IsUnique();
      b.Entity<NewsPost>().HasIndex(n => new { n.AgencyId, n.Slug }).IsUnique();
    }
  }
}
