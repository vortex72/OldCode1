namespace EPWI.Components.Models
{
  public class KitCatalogManufacturerKit
  {
    public string KitID { get; set; }
    public string Description { get; set; }
    public string StartYear { get; set; }
    public string EndYear { get; set; }
    public string Displacement { get; set; }
    public string Cylinders { get; set; }
    public bool IsEDirectKitAvailable { get; set; }
    public bool UseableFlag { get; set; }
    public string DisplayName
    {
      get
      {
        if (UseableFlag)
        {
          return $"USE {KitID}";
        }
        else
        {
          return KitID;
        }
      }
    }
  }
}
