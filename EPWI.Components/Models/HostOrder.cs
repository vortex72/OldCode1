namespace EPWI.Components.Models
{
	/// <summary>
	/// A POCO object for submitting orders to the host
	/// </summary>
	public class HostOrder
	{
		public int OrderID { get; set; }
		public System.Nullable<int> InvoiceNumber { get; set; }
		public int UserID { get; set; }
		public int EPWCustID { get; set; }
		public char EPWCompCode { get; set; }
		public System.Nullable<int> BillToCustID { get; set; }
		public string SoldToName { get; set; }
		public string SoldToAddress1 { get; set; }
		public string SoldToAddress2 { get; set; }
		public string SoldToCity { get; set; }
		public string SoldToState { get; set; }
		public string SoldToZip { get; set; }
		public string SoldToZip4 { get; set; }
		public string SoldToPhone { get; set; }
		public string UserName { get; set; }
		public System.Nullable<char> PORequired { get; set; }
		public string PONumber { get; set; }
		public System.Nullable<char> TermsCode { get; set; }
		public System.Nullable<int> PriceType { get; set; }
		public System.Nullable<char> CCOnFile { get; set; }
		public System.Nullable<char> Taxable { get; set; }
		public System.Nullable<decimal> TaxValue { get; set; }
		public System.Nullable<decimal> TaxPercent { get; set; }
		public System.Nullable<decimal> SubTotal { get; set; }
		public System.Nullable<decimal> SpecialCharges { get; set; }
		public System.Nullable<decimal> OrderTotal { get; set; }
		public string ShipToName { get; set; }
		public string ShipToAddress1 { get; set; }
		public string ShipToAddress2 { get; set; }
		public string ShipToCity { get; set; }
		public string ShipToState { get; set; }
		public string ShipToZip { get; set; }
		public string ShipToZip4 { get; set; }
		public string ShipToPhone { get; set; }
		public bool AllowDropShip { get; set; }
		public string AssignedWhse { get; set; }
		public string PreferredWhse { get; set; }
		public string DefaultShipMethod { get; set; }
		public string DefaultShipMethodName { get; set; }
		public string RequestedShipMethod { get; set; }
		public string Comments1 { get; set; }
		public string Comments2 { get; set; }
		public string AltContact1 { get; set; }
		public string AltContact2 { get; set; }
		public System.Nullable<char> OrderStatus { get; set; }
		public System.Nullable<char> StatusFlag1 { get; set; }
		public System.Nullable<char> StatusFlag2 { get; set; }
		public System.Nullable<char> StatusFlag3 { get; set; }
		public bool ManualHandling { get; set; }
		public string ManualReason1 { get; set; }
		public string ManualReason2 { get; set; }
		public string ManualReason3 { get; set; }
		public string ManualReason4 { get; set; }
		public System.DateTime CreateDate { get; set; }
		public System.Nullable<System.DateTime> EnteredDate { get; set; }
		public bool SendEmail { get; set; }
		public int QuoteNumber { get; set; }
	}
}