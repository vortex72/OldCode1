using System;
using System.Linq;

namespace EPWI.Components.Models
{
    public partial class User : ICustomerData
    {
        public string[] Roles
        {
            get
            {
                var roles = (from rm in RoleMemberships
                    select rm.Role.RoleKey).ToList();

                // add a virtual role, "PRICING_ACCESS", which the user is in if they are a customer and not in the limited pricing role
                if (!roles.Contains("LIMITED_PRICING") && roles.Contains("CUSTOMER"))
                {
                    roles.Add("PRICING_ACCESS");
                }
                return roles.ToArray();
            }
        }

        public bool DisplayPriceType(int view, PriceType priceType)
        {
            var bitMask = (view == 1 ? PricingDisplayBitmask : PricingDisplayBitmask2);

            return (bitMask & (int) priceType) == (int) priceType;
        }

        #region ICustomerData Members

        //public string EmailAddress
        //{
        //  get
        //  {
        //    return this.UserName;
        //  }
        //  set
        //  {
        //    throw new NotImplementedException();
        //  }
        //}

        public bool AccessInvoiceCost
        {
            get { return Roles.Contains("ACCESS_INVOICE_COST"); }
            set { throw new NotImplementedException(); }
        }

        public bool AccessJobberCost
        {
            get { return Roles.Contains("ACCESS_JOBBER_COST"); }
            set { throw new NotImplementedException(); }
        }

        public bool AccessEliteCost
        {
            get { return Roles.Contains("ACCESS_ELITE_COST"); }
            set { throw new NotImplementedException(); }
        }


        partial void OnCreated()
        {
            // supply defaults
            if (PricingFactor == 0)
                PricingFactor = 1.35M;

            if (PricingDisplayBitmask == (int) PriceType.Undefined)
                PricingDisplayBitmask = (int) PriceType.Customer | (int) PriceType.Jobber | (int) PriceType.Invoice | (int) PriceType.Elite;

            if (PricingDisplayBitmask2 == (int) PriceType.Undefined)
                PricingDisplayBitmask = (int) PriceType.Customer | (int) PriceType.Jobber | (int) PriceType.Invoice | (int) PriceType.Elite;

            if (CustomerPricingBasis == (int) PriceType.Undefined)
                CustomerPricingBasis = PriceType.Jobber;

            if (MarginPricingBasis == (int) PriceType.Undefined)
                MarginPricingBasis = PriceType.Jobber;
        }

        #endregion
    }
}