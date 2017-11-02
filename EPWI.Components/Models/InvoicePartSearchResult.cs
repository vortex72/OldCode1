using System;

namespace EPWI.Components.Models
{
  public class InvoicePartSearchResult
  {
    public bool ShipmentPending { get; set; }
    public DateTime InvoiceDate { get; set; } 
    public string InvoiceNumber { get; set; }
    public string LineCode { get; set; }
    public string ItemNumber { get; set; }
    public string SizeCode { get; set; }
    public string Type { get; set; }
    public DateTime OrderDate { get; set; }
    public string Warehouse { get; set; }
    public decimal IndividualPrice { get; set; } //NOTE: Not used right now
    public decimal TotalPrice { get; set; }
    public int Quantity { get; set; }
    public string Kit { get; set; }
  }
}
