using System.Collections.Generic;

namespace EPWI.Components.Models
{
	public class StockStatusRequest
	{
		public int UserID { get; set; }
		public string ItemNumber { get; set; }
		public string LineCode { get; set; }
		public int NIPCCode { get; set; }
		public int Quantity { get; set; }
		public string SizeCode { get; set; }
		public decimal Price { get; set; }
		public decimal DiscountPercent { get; set; }
		public int CoreNIPC { get; set; }
		public decimal CorePrice { get; set; }
    public Dictionary<string, int> WarehouseAvailability { get; set; } 

		public void Store(int userId, string itemNumber, string lineCode, int nipcCode, int quantity, string sizeCode, decimal price, decimal discountPercent, int coreNipc, decimal corePrice)
		{
			UserID = userId;
			ItemNumber = itemNumber;
			LineCode = lineCode;
			NIPCCode = nipcCode;
			Quantity = quantity;
			SizeCode = sizeCode;
			Price = price;
			DiscountPercent = discountPercent;
			CoreNIPC = coreNipc;
			CorePrice = corePrice;
		}

		public bool IsPopulated
		{
			get
			{
				return !string.IsNullOrEmpty(ItemNumber);
			}
		}

    public override string ToString()
    {
      return
          $"UserID: {this.UserID}; ItemNumber: {this.ItemNumber}; LineCode: {this.LineCode}; NIPCCode: {this.NIPCCode}; Quantity: {this.Quantity}; SizeCode: {this.SizeCode}";
    }
	}
}
