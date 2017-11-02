namespace EPWI.Components.Models
{
  public class InvoiceDetail
  {
    public string KitComponent { get; set; }
    public string LineCode { get; set; }
    public int SequenceNumber { get; set; }
    public string ItemNumber { get; set; }
    public string SizeCode { get; set; }
    public string Description { get; set; }
    public int QuantityOrdered { get; set; }
    public int QuantityShipped { get; set; }
    public string Warehouse { get; set; }
    public string ShipFromWarehouse { get; set; }
    public string ShipFromCompany { get; set; }
    public string PONumber { get; set; }
    public decimal NetPrice { get; set; }
    public string Notes { get; set; }

    public decimal IndividualPrice
    {
      get
      {
        decimal price = 0;

        if (QuantityShipped != 0)
        {
          price = NetPrice / QuantityShipped;
        }
        return price;
      }
    }

    public bool ShowShipFromInfo
    {
      get
      {
        return (!string.IsNullOrEmpty(this.ShipFromWarehouse) && (this.ShipFromWarehouse != this.Warehouse));
      }
    }

  }
}
