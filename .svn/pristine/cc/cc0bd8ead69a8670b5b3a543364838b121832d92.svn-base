using System.Collections.Generic;

namespace EPWI.Components.Models
{
	public class InventoryItem
	{
		public int NIPCCode { get; set; }
		public string ItemNumber { get; set; }
		public string ItemDescription { get; set; }
		public string LineCode { get; set; }
		public string LineDescription { get; set; }
		public int Category { get; set; }
    public CategoryFamily CategoryFamily { get; set; }
		public int Subcategory { get; set; }
		public bool IsYearRequired { get; set; }
		public bool RequiresSpecialDisplay { get; set; }
		public bool IsKTRACK { get; set; }
		public int UnitsPerSellMultiple { get; set; }
		public string ApplicableSize { get; set; }
		public int ApplicableQuantity { get; set; }
		public IEnumerable<string> Sizes { get; set; }
		public bool IsKit
		{
			get
			{
				return this.LineDescription == "KITS";
			}
		}
	}
}
