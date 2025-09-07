namespace GovSite.Models
{
  public class Template
  {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string LayoutPath { get; set; } = "";     // مثال: "~/Views/Shared/Templates/_ClassicLayout.cshtml"
    public string CssFile { get; set; } = "";        // مثال: "/templates/classic.css"
    public string PreviewImageUrl { get; set; } = ""; // اختياري
  }
}
