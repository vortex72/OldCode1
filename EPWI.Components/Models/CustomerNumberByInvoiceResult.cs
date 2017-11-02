namespace EPWI.Components.Models
{
  public class CustomerNumberByInvoiceResult
  {
    public char CompanyCode { get; set; }
    public int CustomerID { get; set; }

    public bool DeletedInvoice
    {
      get { return CompanyCode == 'D' && CustomerID == 0; }
    }

    public bool OpenInvoice
    {
      get { return CompanyCode == 'O' && CustomerID == 0; }
    }

    public bool InvoiceNotFound
    {
      get { return CompanyCode == 'E' && CustomerID == 0; }
    }
  }
}