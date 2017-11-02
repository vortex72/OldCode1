using EPWI.Components.Models;

namespace EPWI.Components.Utility
{
  public static class PriceUtility
  {
    public static decimal Adjusted(this decimal price, int view, ICustomerData customerData)
    {
      if (view == 2)
      {
        price *= customerData.PricingFactor;
        if (customerData.CustomerPricingBasis == PriceType.Elite)
        { // 10% discount for elite
          price *= 0.9M;
        }
      }

      return price;
    }
  }
}
