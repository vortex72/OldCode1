using System.Collections.Generic;
using xVal.ServerSide;
using EPWI.Components.Models;
using System.Web.Mvc;
using Castle.Core.Internal;

namespace EPWI.Web.Models
{
  [Bind(Include = "RequestedQuantity, RequestedItemNumber, RequestedSize, RequestedLineCode, CurrentView, OriginalPartUniqueIdentifier, OriginalPartPrice, KitNipc")]
  public class KitStockStatusViewModel : StockStatusViewModel
  {
    public int KitNipc { get; set; }
    public string OriginalPartUniqueIdentifier { get; set; }
    public InventoryItem OriginalPartInventoryItem { get; set; }
    public decimal OriginalPartPrice { get; set; }

    public bool IsForInterchange => !string.IsNullOrEmpty(OriginalPartUniqueIdentifier);

      public int OriginalPartNIPC
          => OriginalPartUniqueIdentifier.IsNullOrEmpty() ? -1 : int.Parse(OriginalPartUniqueIdentifier.Split('-')[0]);

      public decimal PriceDifference(decimal price)
    {
      return (price - this.OriginalPartPrice) * this.RequestedQuantity.Value;
    }

    protected override IEnumerable<ErrorInfo> GetRuleViolations()
    { 
      var errors = new List<ErrorInfo>();
      
      // perform additional checks that apply to Kit Part Interchange Stock Status Searches
      if (IsForInterchange)
      {
        if (OriginalPartNIPC == this.InventoryItem.NIPCCode)
        {
          errors.Add(new ErrorInfo("RequestedItemNumber", "You can not substitute the exact same part as the original kit part."));
          errors.Add(new ErrorInfo("RequestedItemNumber", "Please select another part number to substitute, or close this window to return to the kit builder."));
          CanPlaceOrder = false;
        }
        if (this.OriginalPartInventoryItem.CategoryFamily != null && this.InventoryItem.CategoryFamily != null && this.OriginalPartInventoryItem.CategoryFamily.CategoryID != this.InventoryItem.CategoryFamily.CategoryID)
        {
          errors.Add(new ErrorInfo("RequestedItemNumber",
              $"The selected item is not in the same category as the original kit part. The original part is in the '{this.OriginalPartInventoryItem.CategoryFamily.CategoryName}' category while the selected part is in the '{this.InventoryItem.CategoryFamily.CategoryName}' category. Please select another part number to substitute, or close this window to return to the kit screen."));
          CanPlaceOrder = false;
        }
      }

      if (this.InventoryItem.IsKit)
      {
        errors.Add(new ErrorInfo("RequestedItemNumber", "This part number is for a complete kit. Other kits cannot be added to the existing kit."));
        CanPlaceOrder = false;
      }

      if (this.StockStatus != null && this.StockStatus.CorePrice > 0)
      {
        errors.Add(new ErrorInfo("RequestedItemNumber", "Items requiring cores are not allowed to be added to kits. Please use the standard stock status screen to add this part."));
        CanPlaceOrder = false;
      }

      errors.AddRange(base.GetRuleViolations());

      return errors;
    }

  }
}
