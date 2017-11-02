using System.ComponentModel;

namespace EPWI.Components.Models
{
	public enum OrderMethod
	{
		[Description("M")]
		MainWarehouse,
		[Description("B")]
		MainAndSecondaryWarehouse,
		[Description("S")]
		SecondaryWarehouse,
		[Description("C")]
		MainAndOtherWarehouse,
		[Description("O")]
		OtherWarehouse,
		[Description("L")]
		LocalPickup,
		[Description("D")]
		DropShip,
		[Description("X")]
		Manual
	}

	public static class OrderMethodExtensions
	{
		public static string ToCode(this OrderMethod val)
		{
			DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
			return attributes.Length > 0 ? attributes[0].Description : string.Empty;
		}
	} 
}
