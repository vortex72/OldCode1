using System.Linq;

namespace EPWI.Components.Models
{
  public class CatalogRepository : Repository
  {
    public IQueryable<Catalog> GetAll()
    {
      return from c in Db.Catalogs
                  select c;
    }
  }
}
