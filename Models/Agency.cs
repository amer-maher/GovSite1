namespace GovSite.Models {
    public class Agency
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;          // moh, moi...
        public string ThemeKey { get; set; } = "default";
        public string? LogoUrl { get; set; }
        public ICollection<Page> Pages { get; set; } = new List<Page>();
        public ICollection<NewsPost> News { get; set; } = new List<NewsPost>();
        public ICollection<Setting> Settings { get; set; } = new List<Setting>();   
         public int? TemplateId { get; set; } 
        public Template? Template { get; set; }

  }
}
