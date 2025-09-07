namespace GovSite.Models {
  public class MediaFile {
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public Agency Agency { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public long SizeBytes { get; set; }
  }
}
