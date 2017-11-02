using System.Collections.Generic;
using System.Linq;

namespace EPWI.Components.Models
{
  public class KitCatalogRepository : Repository
  {
    public IEnumerable<Manufacturer> GetManufacturerList()
    {
      var list = from m in Db.usp_GetKitCatalogManufacturers()
                 select new Manufacturer
                 {
                   Make = m.OMAKE.Trim(),
                   Name = m.OMKNM.Trim()
                 };

      return list;
    }

    public string GetManufacturerNameByMake(string make)
    {
      return (from m in Db.usp_GetKitCatalogManufacturers()
              where (m.OMAKE ?? string.Empty).Trim() == make
              select m.OMKNM).Single();
    }

    public IEnumerable<KitCatalogManufacturerKit> GetManufacturerKits(string make)
    {
      var kits = from k in Db.usp_GetKitCatalogMfrKits(make)
                 select new KitCatalogManufacturerKit
                 {
                   KitID = k.KXEG,
                   Description = k.Description,
                   StartYear = k.StartYear.GetValueOrDefault(0).ToString(),
                   EndYear = k.EndYear.GetValueOrDefault(0).ToString(),
                   Displacement = k.ODISP,
                   Cylinders = k.OCYL,
                   IsEDirectKitAvailable = k.IsEDirectKitAvail,
                   UseableFlag = k.UseableFlag
                 };
      return kits;
    }
  }
}
