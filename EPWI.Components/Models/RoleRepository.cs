using System.Linq;

namespace EPWI.Components.Models
{
  public class RoleRepository : Repository
  {
    public string[] GetAllRoles()
    {
      return (from r in Db.Roles select r.RoleKey).ToArray();
    }
  }
}
