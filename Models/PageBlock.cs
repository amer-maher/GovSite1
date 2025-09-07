namespace GovSite
{
    public class PageBlock
{
  public string Type { get; set; } = "cards"; // cards | text | features | gallery
  public string? Title { get; set; }
  public string? Subtitle { get; set; }
  public string? BgColor { get; set; }
  public string? TextColor { get; set; }
  public List<BlockItem> Items { get; set; } = new();
}
public class BlockItem
{
  public string? Title { get; set; }
  public string? Text { get; set; }
  public string? ImageUrl { get; set; }
  public string? LinkUrl { get; set; }
}

}