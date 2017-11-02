using System;

namespace EPWI.Components.Models
{
    public interface ICustomerData
    {
        string CompanyName { get; set; }
        char? CompanyCode { get; set; }
        string UserName { get; set; }
        string EmailAddress { get; set; }
        int? CustomerID { get; set; }
        int UserID { get; set; }
        decimal PricingFactor { get; set; }
        PriceType CustomerPricingBasis { get; set; }
        PriceType MarginPricingBasis { get; set; }
        bool AccessInvoiceCost { get; set; }
        bool AccessJobberCost { get; set; }
        bool AccessEliteCost { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        bool DisplayPriceType(int view, PriceType priceType);
    }

    public class MockCustomerData : ICustomerData
    {
        #region ICustomerData Members

        public string CompanyName
        {
            get { return "BLUE RIBBON TECHNOLOGIES"; }
            set { throw new NotImplementedException(); }
        }

        public char? CompanyCode
        {
            get { return 'N'; }
            set { throw new NotImplementedException(); }
        }

        public int? CustomerID
        {
            get { return 116; }
            set { throw new NotImplementedException(); }
        }

        public int UserID
        {
            get { return 1; }
            set { throw new NotImplementedException(); }
        }

        public string UserName
        {
            get { return "EPWITEST"; }
            set { throw new NotImplementedException(); }
        }

        public string EmailAddress
        {
            get { return "testuser@epwi.net"; }
            set { throw new NotImplementedException(); }
        }

        public string FirstName
        {
            get { return "Kevin"; }
            set { throw new NotImplementedException(); }
        }

        public string LastName
        {
            get { return "Krueger"; }
            set { throw new NotImplementedException(); }
        }


        public decimal PricingFactor
        {
            get { return 1.1M; }
            set { throw new NotImplementedException(); }
        }

        public PriceType CustomerPricingBasis
        {
            get { return PriceType.Jobber; }
            set { throw new NotImplementedException(); }
        }

        public PriceType MarginPricingBasis
        {
            get { return PriceType.Jobber; }
            set { throw new NotImplementedException(); }
        }

        public bool AccessInvoiceCost
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        public bool AccessJobberCost
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        public bool AccessEliteCost
        {
            get { return true; }
            set { throw new NotImplementedException(); }
        }

        public bool DisplayPriceType(int view, PriceType priceType)
        {
            return true;
        }

        #endregion
    }
}