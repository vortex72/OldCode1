using System.Collections.Generic;

namespace EPWI.Components.Models
{
	public class KitCategoryMapping
	{
		public static Dictionary<KitCategory, KitCategoryMapping> Mappings = new Dictionary<KitCategory,KitCategoryMapping>();

		public IEnumerable<int> CategoryID { get; set; }
		public IEnumerable<string> KitType { get; set; }

		static KitCategoryMapping()
		{
      Mappings.Add(KitCategory.AdditionalParts, new KitCategoryMapping { CategoryID = new int[] { 0 }, KitType = new string[] { "MK", "EK", "RR", "RRP", "RMK", "CK"} });
			Mappings.Add(KitCategory.Rings, new KitCategoryMapping { CategoryID = new int[] { 15 }, KitType = new string[] { "MK", "EK", "RR", "RRP" } });
			Mappings.Add(KitCategory.RodBearings, new KitCategoryMapping { CategoryID = new int[] { 20 }, KitType = new string[] { "MK", "EK", "RR", "RRP", "RMK"} });
			Mappings.Add(KitCategory.GasketSet, new KitCategoryMapping { CategoryID = new int[] { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 45 }, KitType = new string[] { "MK", "EK", "RR", "RRP" } });
			Mappings.Add(KitCategory.Pistons, new KitCategoryMapping { CategoryID = new int[] { 10,11,100,101 }, KitType = new string[] { "MK", "EK" } });
			Mappings.Add(KitCategory.MainBearings, new KitCategoryMapping { CategoryID = new int[] { 21, 23 }, KitType = new string[] { "MK", "EK", "RRP", "RMK" } });
			Mappings.Add(KitCategory.CamBearings, new KitCategoryMapping { CategoryID = new int[] { 22 }, KitType = new string[] { "MK", "EK" } });
			Mappings.Add(KitCategory.OilPump, new KitCategoryMapping { CategoryID = new int[] { 60, 61 }, KitType = new string[] { "MK", "EK" } });
			Mappings.Add(KitCategory.FreezePlugs, new KitCategoryMapping { CategoryID = new int[] { 110, 111 }, KitType = new string[] { "MK", "EK" } });
			Mappings.Add(KitCategory.PinBushings, new KitCategoryMapping { CategoryID = new int[] { 12 }, KitType = new string[] { "MK", "EK" } });
			Mappings.Add(KitCategory.Camshaft, new KitCategoryMapping { CategoryID = new int[] { 65 }, KitType = new string[] { "MK", "CK" } });
			Mappings.Add(KitCategory.Lifters, new KitCategoryMapping { CategoryID = new int[] { 70 }, KitType = new string[] { "MK", "CK" } });
			Mappings.Add(KitCategory.TimingKit, new KitCategoryMapping { CategoryID = new int[] { 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 }, KitType = new string[] { "MK" } });

		}
	}

}
