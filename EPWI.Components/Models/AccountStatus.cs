using System;

namespace EPWI.Components.Models
{
  public class AccountStatus
  {
    public decimal ELITE_SALES_LEVEL = 2500M;
    public decimal ELITE_DISCOUNT_PERCENT = 0.1M;
    public int ELITE_DISCOUNT_CUTOFF_DAY = 10;

    public ICustomerData CurrentUser { get; set; }
    public int CustomerID { get; set; }
    public string CompanyCode { get; set; }
    public bool ErrorOccurred { get; set; }
    public string ErrorDescription { get; set; }
    public DateTime Date { get; set; }
    public decimal CurrentMonthSales { get; set; }
    public decimal PastDueCurrentMonth { get; set; }
    public decimal PastDueLastMonth { get; set; }
    public decimal PastDue30 { get; set; }
    public decimal PastDue60 { get; set; }
    public decimal PastDue90 { get; set; }
    public decimal LastMonthSales { get; set; }
    public DateTime LastStatementDate { get; set; }
    public decimal PaymentsAndCreditsApplied { get; set; }
    public bool CODCustomer { get; set; } 
    public bool MailStatementToCustomer { get; set; }
    public decimal StatementBalance { get; set; }
    public decimal StatementRemainingBalance { get; set; }
    public decimal MaxDiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal CurrentInvoices { get; set; }
    public decimal CurrentCredits { get; set; }
    public decimal CurrentAdjustments { get; set; }
    public decimal CurrentNSFChecks { get; set; }
    public decimal CurrentNSFCharges { get; set; }
    public decimal CurrentMiscellaenous { get; set; }
    public decimal CurrentPaymentsMade { get; set; }
    public decimal CurrentDiscountsAllowed { get; set; }
    public decimal TotalAccountBalance { get; set; }

    public bool IsElite
    {
      get
      { // customer is elite if sales are greater than or equal to threshold and no amounts 30, 60, or 90 days past due 
        return (this.CurrentMonthSales >= ELITE_SALES_LEVEL && !(this.PastDue30 > 0 || this.PastDue60 > 0 || this.PastDue90 > 0));
      }
    }

    public bool BeforeEliteCutoff
    {
      get
      {
        return DateTime.Now.Day <= ELITE_DISCOUNT_CUTOFF_DAY;
      }
    }
  }
}
