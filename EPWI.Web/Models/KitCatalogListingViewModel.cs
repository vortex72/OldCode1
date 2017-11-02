using System.Collections.Generic;
using EPWI.Components.Models;

namespace EPWI.Web.Models
{
  public class KitCatalogListingViewModel
  {
    public IEnumerable<KitCatalogManufacturerKit> Kits { get; set; }
    public string ManufacturerName { get; set; }
  }
}
