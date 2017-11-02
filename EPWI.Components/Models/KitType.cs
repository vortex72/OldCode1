using System.Collections.Generic;

namespace EPWI.Components.Models
{
  public class KitType
  {
    public string Description { get; set; }
    public string Type { get; set; }

    public static List<KitType> KitTypes
    {
      get
      {
        return new List<KitType> {
          new KitType { Type = "MK", Description = "Master Kit" },
          new KitType { Type = "EK", Description = "Engine Kit" },
          new KitType { Type = "RR", Description = "Re-Ring Kit" },
          new KitType { Type = "CK", Description = "Cam Kit" },
          new KitType { Type = "RRP", Description = "Re-Ring Kit + Main Bearings" },
          new KitType { Type = "RMK", Description = "Rod Bearings + Main Bearings" }
        };
      }
    }
  }
}
