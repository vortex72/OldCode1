using System;
using System.Collections.Generic;

namespace EPWI.Components.Models
{
  public class Invoice
  {
    public bool Error { get; set; }
    public Address CompanyAddress { get; set; }
    public Address CustomerWarehouseAddress { get; set; }
    public Address ShippingWarehouseAddress { get; set; }
    public bool ShipmentPending { get; set; }
    public DateTime ShipDate { get; set; }
    public DateTime CreateDate { get; set; }
    public string CompanyCode { get; set; }
    public int CustomerID { get; set; }
    public string CustomerWarehouse { get; set; }
    public string ShippingWarehouse { get; set; }
    public Address BillToAddress { get; set; }
    public Address SoldToAddress { get; set; }
    public Address ShipToAddress { get; set; }
    public string InvoiceNumber { get; set; }
    public string Associate { get; set; }
    public IEnumerable<string> TrackingNumbers { get; set; }
    public IEnumerable<string> CustomerNotes { get; set; }
    public string Terms { get; set; }
    public string PONumber { get; set; }
    public string OrderNotes1 { get; set; }
    public string OrderNotes2 { get; set; }
    public decimal MerchandiseTotal { get; set; }
    public decimal InvoiceTotal { get; set; }
    public decimal TotalDue { get; set; }
    public string ShippingCarrier { get; set; }
    public string Type { get; set; }

    public IEnumerable<InvoiceDetail> InvoiceDetails { get; set; }

    public bool ShowCustomerWarehouse
    {
      get
      {
        return (this.CustomerWarehouse != "DEN" && this.CustomerWarehouse != "DAL" && this.CustomerWarehouseAddress != null);
      }
    }

    public bool ShowShippingWarehouse
    {
      get
      {
        return (this.ShippingWarehouse != this.CustomerWarehouse && this.CustomerWarehouse != "DEN" && this.CustomerWarehouse != "DAL" && this.ShippingWarehouseAddress != null);
      }
    }

    /* This isn't used right now. InvoiceContent.ascx current determines shipping carrier by parsing the tracking number */
    public ShippingCarrierType ShippingCarrierType
    {
      get
      {
        ShippingCarrierType carrier = ShippingCarrierType.Undefined;
        if (this.ShippingCarrier != null && this.ShippingCarrier.Length > 0)
        {
          switch (ShippingCarrier.ToUpper().Substring(0, 1))
          {
            case "U":
              carrier = ShippingCarrierType.UPS;
              break;
            case "F":
              carrier = ShippingCarrierType.FedEx;
              break;
          }
        }

        return carrier;
      }
    }

    public static IEnumerable<Invoice> GetDummyData()
    {
      return new List<Invoice>();
    }



  }
}
