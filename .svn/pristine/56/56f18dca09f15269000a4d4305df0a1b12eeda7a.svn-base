using System.Linq;

namespace EPWI.Components.Models
{
  public class CategoryFamilyRepository : Repository
  {
    public CategoryFamily GetByCategoryCode(int categoryCode)
    {
      return (from c in Db.CategoryCodes
             where c.CategoryCode1 == categoryCode
             select c.CategoryFamily).SingleOrDefault();
    }
  }
}
