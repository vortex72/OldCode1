using System.Collections.Generic;

namespace EPWI.Components.Models
{
  public class RelatedCategoryMapping
  {
    public static Dictionary<RelatedCategory, RelatedCategoryMapping> Mappings = new Dictionary<RelatedCategory, RelatedCategoryMapping>();

    public IEnumerable<int> CategoryID { get; set; }
    public int? LineType { get; set; }

    static RelatedCategoryMapping()
    {
      Mappings.Add(RelatedCategory.CrankKit, new RelatedCategoryMapping { CategoryID = new int[] { 5, 85 }, LineType = null });
      Mappings.Add(RelatedCategory.IntakeValves, new RelatedCategoryMapping { CategoryID = new int[] { 90 }, LineType = 27 });
      Mappings.Add(RelatedCategory.ExhaustValves, new RelatedCategoryMapping { CategoryID = new int[] { 90 }, LineType = 29 });
      Mappings.Add(RelatedCategory.ValveSprings, new RelatedCategoryMapping { CategoryID = new int[] { 93 }, LineType = null });
      Mappings.Add(RelatedCategory.ValveGuides, new RelatedCategoryMapping { CategoryID = new int[] { 91 }, LineType = null });
      Mappings.Add(RelatedCategory.PushRods, new RelatedCategoryMapping { CategoryID = new int[] { 71 }, LineType = null });
      Mappings.Add(RelatedCategory.RockerArms, new RelatedCategoryMapping { CategoryID = new int[] { 75 }, LineType = null });
      Mappings.Add(RelatedCategory.OilPumpScreens, new RelatedCategoryMapping { CategoryID = new int[] { 62 }, LineType = null });
      Mappings.Add(RelatedCategory.ConnectingRods, new RelatedCategoryMapping { CategoryID = new int[] { 80 }, LineType = null });
      Mappings.Add(RelatedCategory.CylinderSleeves, new RelatedCategoryMapping { CategoryID = new int[] { 100 }, LineType = null });
      Mappings.Add(RelatedCategory.BalancerSleeve, new RelatedCategoryMapping { CategoryID = new int[] { 120 }, LineType = null });
      Mappings.Add(RelatedCategory.SpringShims, new RelatedCategoryMapping { CategoryID = new int[] { 0 }, LineType = 35 });

    }
  }
}
