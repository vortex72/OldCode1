using System.Linq;
using EPWI.Components.Models;

namespace EPWI.Web.Models
{
  public class LineDownloadViewModel
  {
    public IQueryable<Line> Lines { get; set; }
  }
}
