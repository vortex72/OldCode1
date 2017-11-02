using System.Linq;
using EPWI.Components.Models;

namespace EPWI.Web.Models
{
  public class KitModelBase
  {
    public Kit Kit { get; set; }
    public int CurrentView { get; set; }
    public ICustomerData CustomerData { get; set; }
    public bool Editing { get; set; }
    public bool AcesMode { get; set; }
    
    public FulfillmentProcessingResult FulfillmentProcessingResult { get; set; }

    public bool ConfirmingAvailability
    {
      get
      {
        return FulfillmentProcessingResult != null;
      }
    }

    public bool ShowForceToOrderOption
    {
      get
      { // allow user to force to order if there is an X in the fulfillment processing result, indicating one or more items are unavailable
        return ConfirmingAvailability && FulfillmentProcessingResult.ResultCode.Contains('X');
      }
    }

    public bool CrankKitSelected
    {
      get
      {
        return Kit.SelectedCrankKitNIPC > 0;
      }
    }

  }
}
