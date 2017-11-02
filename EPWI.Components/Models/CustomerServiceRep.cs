namespace EPWI.Components.Models
{
  public class CustomerServiceRep
  {
    public int CustomerID { get; set; }
    public char CompanyCode { get; set; }
    public string UserCode { get; set; }
    public string UserName { get; set; }
    public int JobID { get; set; }
    public string LocationCode { get; set; }

    public string FormattedCSR
    {
      get
      {
        return string.Format("{2} - {0} {1}", UserCode, UserName, LocationCode);
      }
    }
  }
}
