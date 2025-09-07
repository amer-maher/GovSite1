namespace GovSite.Models {
  public class NewsPost {
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public Agency Agency { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string? Summary { get; set; }
    public string? HtmlContent { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; } = true;
  }
}
