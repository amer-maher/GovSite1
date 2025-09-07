namespace GovSite.Models {
  public class Setting {
    public int Id { get; set; }
    public int AgencyId { get; set; }
    public Agency Agency { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
  }
}
