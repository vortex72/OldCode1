using System.Collections.Generic;

namespace EPWI.Components.Models
{
  public class InvoicePartSearch
  {
    public ICustomerData CustomerData { get; set; }
    public string PartNumber { get; set; }
    public IEnumerable<InvoicePartSearchResult> Invoices { get; set; }
  }
}
