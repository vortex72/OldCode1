using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPWI.Components.Models;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;
using xVal.ServerSide;

namespace EPWI.Web.Models
{
  [Bind(Include = "RequestedQuantity, RequestedItemNumber, RequestedSize, RequestedLineCode, CurrentView")]
  public class StockStatusViewModel : IDataErrorInfo
  {
    public bool CanPlaceOrder { get; set; }

    public int? RequestedQuantity { get; set; }
    public string RequestedItemNumber { get; set; }
    public string OriginalItemNumber { get; set; }
    public string RequestedSize { get; set; }
    public string RequestedLineCode { get; set; }
    public bool SearchSubmitted { get; private set; }
    public bool SearchCriteriaComplete { get; private set; }
    public int CurrentView { get; set; }
    public StockStatus StockStatus { get; private set; }
    public ICustomerData CustomerData { get; set; }
    public long SavedKitConfigurationID { get; set; }
    public string LastViewedKit { get; set; }
    public string OpticatPartInfoJson { get; set; }

    public IEnumerable<InventoryItem> InventoryItems { get; set; }
    public IEnumerable<Interchange> Interchanges { get; private set; }

    public StockStatusViewModel()
    {
      CanPlaceOrder = true;
      InventoryItems = Enumerable.Empty<InventoryItem>();
      OpticatPartInfoJson = "null";
    }

    public void AddInventoryItems(IEnumerable<InventoryItem> inventoryItems)
    {
      SearchSubmitted = true;
      InventoryItems = inventoryItems;

      // if only one inventory item, populate model with applicable data
      if (InventoryItems.Count() == 1)
      {
        SearchCriteriaComplete = true;

        var item = InventoryItem;

        if (!RequestedQuantity.HasValue || RequestedQuantity.Value <= 0)
        {
          RequestedQuantity = 1;
        }

        if (item.ApplicableQuantity > 0)
        {
          RequestedQuantity = item.ApplicableQuantity;
        }

        if (!string.IsNullOrEmpty(item.ApplicableSize))
        {
          RequestedSize = item.ApplicableSize;
        }

        OriginalItemNumber = RequestedItemNumber;
        RequestedItemNumber = item.ItemNumber;

        //Added 10/16/14: Adjust line code. This fixes invalid Mfr Codes from Opticat
        RequestedLineCode = item.LineCode;

        if (this is KitStockStatusViewModel)
        { // for doing a kit stock status, get additional category information so we can validate that an interchange is between parts of the same category family
          var tempModel = this as KitStockStatusViewModel;

          if (tempModel.OriginalPartInventoryItem != null)
          {
            var categoryFamilyRepository = new CategoryFamilyRepository();

            tempModel.OriginalPartInventoryItem.CategoryFamily = categoryFamilyRepository.GetByCategoryCode(tempModel.OriginalPartInventoryItem.Category);
            tempModel.InventoryItem.CategoryFamily = categoryFamilyRepository.GetByCategoryCode(tempModel.InventoryItem.Category);
          }
        }

        var errors = GetRuleViolations();

        if (errors.Count() > 0)
        {
          throw new RulesException(errors);
        }
      }
    }

    protected virtual IEnumerable<ErrorInfo> GetRuleViolations()
    {
      var errors = new List<ErrorInfo>();

      if (InventoryItem.UnitsPerSellMultiple > 1)
      {
        if (RequestedQuantity % InventoryItem.UnitsPerSellMultiple > 0)
        {
          errors.Add(new ErrorInfo("RequestedQuantity",
              $"The item selected is priced per each but must be purchased in sets. Please order in multiples of {InventoryItem.UnitsPerSellMultiple}."));
          CanPlaceOrder = false;
        }
      }

      return errors;
    }

    public InventoryItem InventoryItem
    {
      get
      {
        if (InventoryItems.Count() == 1)
        {
          return InventoryItems.First();
        }
        else
        {
          throw new InvalidOperationException("Can access Inventory Item property when there is one and only one matching Inventory Item.");
        }
      }
    }

    public bool ItemNumberChanged => SearchCriteriaComplete && (RequestedItemNumber != OriginalItemNumber);

      public bool NeedToSelectLineCode
    {
      get
      {
        bool result = false;

        if (InventoryItems != null)
        {
          result = InventoryItems.Count() > 1;
        }

        return result;
      }
    }

    public bool NeedToSelectSize
    {
      get
      {
        bool result = false;

        if (InventoryItems.Count() == 1)
        {
          result = (string.IsNullOrEmpty(RequestedSize) && InventoryItem.Sizes.Any());
        }

        return result;
      }
    }

    public bool RequestComplete => InventoryItems.Count() == 1 && !NeedToSelectSize && CanPlaceOrder;

      public bool ShowInterchanges => (Interchanges != null && Interchanges.Any());

      public string RequestedLineCodeDescription
    {
      get
      {
        string description = string.Empty;

        if (InventoryItems.Count() == 1)
        {
          description = InventoryItem.LineDescription;
        }

        return description;

      }
    }

    public void AddStockStatus(StockStatus stockStatus)
    {
      StockStatus = stockStatus;

      if (stockStatus.DenyPurchase)
      {
        CanPlaceOrder = false;
        throw new RulesException("RequestedItemNumber", "MSD and Superchips require that you are registered as an authorized dealer before you can purchase from any supplier.");
      }
    }

    public void AddInterchanges(IEnumerable<Interchange> interchanges)
    {
      Interchanges = interchanges;
    }

    public IEnumerable<string> GetMessages(RequestContext requestContext)
    {
      var messages = new List<string>();
      bool kitRequest = this is KitStockStatusViewModel;

      if (SearchSubmitted && InventoryItems.Count() == 0)
      {
        messages.Add("The Part Number Entered is Invalid. Please Re-Enter.");
      }

      if (NeedToSelectSize)
      {
        messages.Add("Multiple Sizes Exist for this Part Number");
      }

      if (NeedToSelectLineCode)
      {
        messages.Add("Multiple Product Lines Exist for this Part Number");
      }

      if (RequestComplete)
      {
        if (StockStatus.IsObsolete)
        {
          messages.Add("Obsolete Item - Non Returnable");
        }

        if (StockStatus.IsSuperseded && !string.IsNullOrEmpty(StockStatus.SupersededPartNumber))
        {
          if (kitRequest)
          {
            // don't generate the message here. The KitSearch view will render the proper form to allow the user to navigate to the replacement part
          }
          else
          {
            string partString = HtmlHelper.GenerateLink(requestContext, RouteTable.Routes,
                $"{StockStatus.SupersededPartLine.Trim()} {StockStatus.SupersededPartNumber.Trim()}", string.Empty, "Search", "StockStatus", new RouteValueDictionary { { "RequestedQuantity", RequestedQuantity }, { "RequestedItemNumber", StockStatus.SupersededPartNumber.Trim() }, { "RequestedLineCode", StockStatus.SupersededPartLine.Trim() } }, null);
            messages.Add($"This Part is Superseded by: {partString}");
          }

          messages.Add("The Information Above is for the Original Part");
          
          // don't show any other messages for Superseded parts
          return messages;
        }

        // display messages for normal kits
        if (InventoryItem.IsKit && !StockStatus.IsCrankKit)
        {
          messages.Add("Pricing is for a standard kit with no substitutions or deletions.");
          if (HttpContext.Current.User.IsInRole("KIT_BUILDER"))
          {
            // No message for now
          }
          else
          {
            messages.Add(
                $"Please call EPWI in {StockStatus.CustomerDefaultWarehouse} if you wish to customize this kit.");
          }
          // don't show any other messages for kits 
          return messages;
        }

        if (StockStatus.IsCrankKit)
        {
          messages.Add("Pricing is for a standard crank kit with no substitutions or deletions.");
          messages.Add($"Core pricing for this crank kit is {CorePriceBasedOnView.ToString("C")}");
          //don't show any other messages for crank kits
          return messages;
        }

        if (StockStatus.OrderMethodAvailability(OrderMethod.MainWarehouse))
        {
          messages.Add($"There is Sufficient Quantity in the {StockStatus.CustomerDefaultWarehouse} Warehouse");
        }
        if (StockStatus.OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) && !kitRequest)
        {
          messages.Add(
              $"There is Sufficient Quantity When Shipped From Both The {StockStatus.CustomerDefaultWarehouse} And The {StockStatus.CustomerSecondaryWarehouse} Warehouses");
        }
        if (StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse))
        {
          string messagePrefix = !StockStatus.OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) ? "There is Sufficent Quantity" : "Or";
          messages.Add(
              $"{messagePrefix} When Shipped From Just Your Secondary Warehouse ({StockStatus.CustomerSecondaryWarehouse})");
        }
        if (StockStatus.OrderMethodAvailability(OrderMethod.OtherWarehouse) || (((StockStatus.OrderMethodAvailability(OrderMethod.Manual) && StockStatus.IsQuantityAvailableCombined) || StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse)) && !StockStatus.OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse)))
        {
          messages.Add($"There is Insufficient Quantity in the {StockStatus.CustomerDefaultWarehouse} Warehouse");
        }
        if (StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse) && !kitRequest)
        {
          messages.Add(
              $"Sufficient Quantity Exists When Shipped From {StockStatus.CustomerDefaultWarehouse} And One Of The Listed Locations");
        }
        if (StockStatus.OrderMethodAvailability(OrderMethod.OtherWarehouse))
        {
          messages.Add("Sufficient Quantity Exists at the Locations Listed");
        }
        if (StockStatus.OrderMethodAvailability(OrderMethod.Manual) && !StockStatus.OrderMethodAvailability(OrderMethod.MainAndOtherWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.MainAndSecondaryWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse))
        {
          if (StockStatus.IsQuantityAvailableCombined && !kitRequest)
          {
            messages.Add("There is Sufficient Quantity When Combined With Other Warehouses");
          }
          else if (!kitRequest) 
          {
            messages.Add("Sorry, There is Insufficient Quantity Available");
          }
          messages.Add("You May Add This Item To Your Order, but It Will Require Manual Processing");
        }
        // since kit requests have different rules (only main, secondary, or other warehouse order method allowed), check for that scenario so we can display an appropriate message
        if (kitRequest && !StockStatus.OrderMethodAvailability(OrderMethod.MainWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.SecondaryWarehouse) && !StockStatus.OrderMethodAvailability(OrderMethod.OtherWarehouse))
        {
          messages.Add("Sorry, There is Insufficient Quantity Available");
        }

        if (StockStatus.CorePrice > 0)
        {
          messages.Add($"Core pricing for this part is {CorePriceBasedOnView.ToString("C")}");
        }
      }

      return messages;
    }

    public void UpdateStockStatusRequest(StockStatusRequest stockStatusRequest)
    {
      stockStatusRequest.UserID = CustomerData.UserID;
      stockStatusRequest.ItemNumber = InventoryItem.ItemNumber;
      stockStatusRequest.LineCode = InventoryItem.LineCode;
      stockStatusRequest.NIPCCode = InventoryItem.NIPCCode;
      stockStatusRequest.Quantity = RequestedQuantity.Value;
      stockStatusRequest.SizeCode = RequestedSize;
      stockStatusRequest.WarehouseAvailability = StockStatus.WarehouseAvailabilityDictionary;

      // Store the kit price if available as opposed to the P3 price
      if (StockStatus.ApplicableKitNipc > 0 && StockStatus.Price[PriceType.KitPrice] > 0)
      {
        stockStatusRequest.Price = StockStatus.Price[PriceType.KitPrice];
        // Don't record the discount percentage because the Kit Price already has the discount percentage applied
        stockStatusRequest.DiscountPercent = 0;
      }
      else
      {
        stockStatusRequest.Price = StockStatus.Price[PriceType.P3];
        stockStatusRequest.DiscountPercent = StockStatus.DiscountPercent;
      }

      stockStatusRequest.CoreNIPC = StockStatus.CoreNIPC;
      stockStatusRequest.CorePrice = StockStatus.CorePrice;
    }

    public override string ToString()
    {
      return
          $"ItemNumber: {RequestedItemNumber}; LineCode: {RequestedLineCode}; Quantity: {RequestedQuantity}; SizeCode: {RequestedSize}";
    }

    private decimal CorePriceBasedOnView => CurrentView == 2 ? StockStatus.CorePrice * CustomerData.PricingFactor : StockStatus.CorePrice;

      #region IDataErrorInfo Members

    public string Error => null;

      public string this[string columnName]
    {
      get
      {
        if (columnName == "RequestedItemNumber" && string.IsNullOrEmpty(RequestedItemNumber))
          return "Item Number is required";
        if (columnName == "RequestedItemNumber" && RequestedItemNumber.Length > 25)
          return "Item Number must be 25 characters or less";
        if (columnName == "RequestedQuantity" && !RequestedQuantity.HasValue)
          return "Quantity must be numeric";
        if (columnName == "RequestedQuantity" && RequestedQuantity.GetValueOrDefault(0) > 999)
          return "Quantity cannot be greater than 999";
        return null;
      }
    }

    #endregion
  }
}
