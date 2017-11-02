using System.Collections.Generic;
using System.Linq;
using EPWI.Components.Models;

namespace EPWI.Web.Models
{
  public class KitInterchangeViewModel : KitModelBase
  {
    public KitPart OriginalPart { get; set; }
    public StockStatus InterchangeStockStatus { get; set; }
    public InventoryItem InterchangeInventoryItem { get; set; }
    public IEnumerable<Interchange> Interchanges { get; set; }
    public bool ShowOriginalPart { get; set; }
    public string DefaultWarehouse { get; set; }
    public string InterchangeCode { get; set; }

    public bool InterchangeItemExists
    {
      get
      {
        return InterchangeInventoryItem != null;
      }
    }
    
    public decimal GetPriceDifference(Interchange interchange)
    {
      if (interchange.NIPCCode == OriginalPart.NIPCCode)
      { // if item is original item, price difference is always 0. We still may need to display the item if it is not available
        // in the customer's default warehouse
        return 0M;
      }

      decimal priceDifference = (interchange.Price[PriceType.Invoice] * interchange.InterchangeQuantity) - (OriginalPart.Price * OriginalPart.QuantitySelected);

      if (this.ConfirmingAvailability && interchange.InterchangeCode == "E")
      { // at the final stage, if the interchange is an "Exact Interchange" and the price difference is less than 200% of the original part, there is no price change
        if (((interchange.Price[PriceType.Invoice] - OriginalPart.Price) / OriginalPart.Price) < 2)
        {
          priceDifference = 0;
        }
      }

      return priceDifference;
    }

    public bool InterchangesExist
    {
      get
      {
        return Interchanges.Where(i => i.NIPCCode != OriginalPart.NIPCCode || ShowOriginalPart).Count() > 0;
      }
    }
  }
}
