using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPWI.Components.Models
{
  public class AccountSettings
  {
    public AccountSettings() { }

    public AccountSettings(User user)
    {
      CustomerPricingBasis = (PriceType) Enum.ToObject(typeof (PriceType), user.CustomerPricingBasis);
      MarginPricingBasis = (PriceType)Enum.ToObject(typeof(PriceType), user.CustomerPricingBasis);
      PricingFactor = Convert.ToInt32(user.PricingFactor * 100);
      UserName = user.UserName;
      PriceTypeSelectionsView1 = new Dictionary<PriceType, bool>();
      PriceTypeSelectionsView2 = new Dictionary<PriceType, bool>();
      populatePriceTypeSelections(user);
    }

    public string UserName { get; set; }
    public PriceType CustomerPricingBasis { get; set; }
    public PriceType MarginPricingBasis { get; set; }
    [Required(ErrorMessage="Pricing Factor is required.")]
    public int PricingFactor { get; set; }
    public Dictionary<PriceType, bool> PriceTypeSelectionsView1 { get; set; }
    public Dictionary<PriceType, bool> PriceTypeSelectionsView2 { get; set; }

    private void populatePriceTypeSelections(User user)
    {
        var mask = (PriceType)Enum.ToObject(typeof (PriceType), user.PricingDisplayBitmask);
      // view 1
      PriceTypeSelectionsView1.Add(PriceType.Customer, getPriceTypeSelection(mask, PriceType.Customer));
      PriceTypeSelectionsView1.Add(PriceType.Jobber, getPriceTypeSelection(mask, PriceType.Jobber));
      PriceTypeSelectionsView1.Add(PriceType.Invoice, getPriceTypeSelection(mask, PriceType.Invoice));
      PriceTypeSelectionsView1.Add(PriceType.Elite, getPriceTypeSelection(mask, PriceType.Elite));
      PriceTypeSelectionsView1.Add(PriceType.Market, getPriceTypeSelection(mask, PriceType.Market));
      PriceTypeSelectionsView1.Add(PriceType.Margin, getPriceTypeSelection(mask, PriceType.Margin));

      // view 2
      mask = (PriceType)Enum.ToObject(typeof (PriceType), user.PricingDisplayBitmask2);
      PriceTypeSelectionsView2.Add(PriceType.Customer, getPriceTypeSelection(mask, PriceType.Customer));
      PriceTypeSelectionsView2.Add(PriceType.Jobber, getPriceTypeSelection(mask, PriceType.Jobber));
      PriceTypeSelectionsView2.Add(PriceType.Invoice, getPriceTypeSelection(mask, PriceType.Invoice));
      PriceTypeSelectionsView2.Add(PriceType.Elite, getPriceTypeSelection(mask, PriceType.Elite));
      PriceTypeSelectionsView2.Add(PriceType.Market, getPriceTypeSelection(mask, PriceType.Market));
      PriceTypeSelectionsView2.Add(PriceType.Margin, getPriceTypeSelection(mask, PriceType.Margin));
    }

    private bool getPriceTypeSelection(PriceType bitmask, PriceType priceType)
    {
      return (bitmask & priceType) == priceType;
    }
  }
}
