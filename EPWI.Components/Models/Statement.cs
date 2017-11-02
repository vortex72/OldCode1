using System;
using System.Collections.Generic;

namespace EPWI.Components.Models
{
  public class Statement
  {
    public ICustomerData CurrentUser { get; set; }
    public bool Error { get; set; }
    public string CompanyCode { get; set; }
    public string Name { get; set; }
    public int CustomerID { get; set; }
    public Address CustomerAddress { get; set; }
    public Address CompanyAddress { get; set; }
    public DateTime StatementDate { get; set; }
    public decimal PreviousBalance { get; set; }
    public decimal TotalDue { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal PastDue30 { get; set; }
    public decimal PastDue60 { get; set; }
    public decimal PastDue90 { get; set; }
    public decimal PastDue120 { get; set; }
    public IEnumerable<string> CustomerNotes { get; set; }

    public IEnumerable<StatementDetail> StatementDetails { get; set; }
  }
}
