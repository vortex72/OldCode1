using System;
using System.Collections.Generic;
using System.Linq;

namespace EPWI.Components.Models
{
  public class FlatInvoice
  {
    private Invoice invoice;
    private InvoiceDetail detail;

    public FlatInvoice()
    {

    }

    public FlatInvoice(Invoice invoice, InvoiceDetail detail)
    {
      this.invoice = invoice;
      //invoice.InvoiceDetails = null;
      this.detail = detail;
    }

    // invoice properties
    public bool Error { get { return invoice.Error; } }
    public string CompanyStreetAddress1 { get { return invoice.CompanyAddress.StreetAddress1; } }
    public string CompanyStreetAddress2 { get { return invoice.CompanyAddress.StreetAddress2; } }
    public string CompanyCity { get { return invoice.CompanyAddress.City; } }
    public string CompanyState { get { return invoice.CompanyAddress.State; } }
    public string CompanyZip { get { return invoice.CompanyAddress.Zip; } }
    public string CompanyPhone { get { return invoice.CompanyAddress.Phone; } }
    public string CompanyAlternatePhone { get { return invoice.CompanyAddress.AlternatePhone; } }
    public string CompanyFax { get { return invoice.CompanyAddress.Fax; } }
    public string CustomerWarehouseStreetAddress1 { get { return invoice.CustomerWarehouseAddress.StreetAddress1; } }
    public string CustomerWarehouseStreetAddress2 { get { return invoice.CustomerWarehouseAddress.StreetAddress2; } }
    public string CustomerWarehouseCity { get { return invoice.CustomerWarehouseAddress.City; } }
    public string CustomerWarehouseState { get { return invoice.CustomerWarehouseAddress.State; } }
    public string CustomerWarehouseZip { get { return invoice.CustomerWarehouseAddress.Zip; } }
    public string CustomerWarehousePhone { get { return invoice.CustomerWarehouseAddress.Phone; } }
    public string CustomerWarehouseAlternatePhone { get { return invoice.CustomerWarehouseAddress.AlternatePhone; } }
    public string CustomerWarehouseFax { get { return invoice.CustomerWarehouseAddress.Fax; } }
    public string ShippingWarehouseStreetAddress1 { get { return invoice.ShippingWarehouseAddress.StreetAddress1; } }
    public string ShippingWarehouseStreetAddress2 { get { return invoice.ShippingWarehouseAddress.StreetAddress2; } }
    public string ShippingWarehouseCity { get { return invoice.ShippingWarehouseAddress.City; } }
    public string ShippingWarehouseState { get { return invoice.ShippingWarehouseAddress.State; } }
    public string ShippingWarehouseZip { get { return invoice.ShippingWarehouseAddress.Zip; } }
    public string ShippingWarehousePhone { get { return invoice.ShippingWarehouseAddress.Phone; } }
    public string ShippingWarehouseAlternatePhone { get { return invoice.ShippingWarehouseAddress.AlternatePhone; } }
    public string ShippingWarehouseFax { get { return invoice.ShippingWarehouseAddress.Fax; } }
    public bool ShipmentPending { get { return invoice.ShipmentPending; } }
    public DateTime ShipDate { get { return invoice.ShipDate; } }
    public DateTime CreateDate { get { return invoice.CreateDate; } }
    public string CompanyCode { get { return invoice.CompanyCode; } }
    public int CustomerID { get { return invoice.CustomerID; } }
    public string CustomerWarehouse { get { return invoice.CustomerWarehouse; } }
    public string ShippingWarehouse { get { return invoice.ShippingWarehouse; } }
    public Address BillToAddress { get { return invoice.BillToAddress; } }
    public Address SoldToAddress { get { return invoice.SoldToAddress; } }
    public Address ShipToAddress { get { return invoice.ShipToAddress; } }
    public string InvoiceNumber { get { return invoice.InvoiceNumber; } }
    public string Associate { get { return invoice.Associate; } }
    public string TrackingNumbers { get { return string.Join(",", invoice.TrackingNumbers.ToArray()); } }
    public string Terms { get { return invoice.Terms; } }
    public string PONumber { get { return invoice.PONumber; } }
    public string OrderNotes1 { get { return invoice.OrderNotes1; } }
    public string OrderNotes2 { get { return invoice.OrderNotes2; } }
    public decimal MerchandiseTotal { get { return invoice.MerchandiseTotal; } }
    public decimal InvoiceTotal { get { return invoice.InvoiceTotal; } }
    public decimal TotalDue { get { return invoice.TotalDue; } }
    public string ShippingCarrier { get { return invoice.ShippingCarrier; } }
    public string Type { get { return invoice.Type; } }
    public bool ShowCustomerWarehouse { get { return invoice.ShowCustomerWarehouse; } }
    public bool ShowShippingWarehouse { get { return invoice.ShowShippingWarehouse; } }
    public ShippingCarrierType ShippingCarrierType { get { return invoice.ShippingCarrierType; } }

    // detail properties
    public string KitComponent { get { return detail.KitComponent; } }
    public string LineCode { get { return detail.LineCode; } }
    public int SequenceNumber { get { return detail.SequenceNumber; } }
    public string ItemNumber { get { return detail.ItemNumber; } }
    public string SizeCode { get { return detail.SizeCode; } }
    public string Description { get { return detail.Description; } }
    public int QuantityOrdered { get { return detail.QuantityOrdered; } }
    public int QuantityShipped { get { return detail.QuantityShipped; } }
    public string Warehouse { get { return detail.Warehouse; } }
    public string ShipFromWarehouse { get { return detail.ShipFromWarehouse; } }
    public string ShipFromCompany { get { return detail.ShipFromCompany; } }
    public string DetailPONumber { get { return detail.PONumber; } }
    public decimal NetPrice { get { return detail.NetPrice; } }
    public string Notes { get { return detail.Notes; } }
    public decimal IndividualPrice { get { return detail.IndividualPrice; }}
    public bool ShowShipFromInfo { get { return detail.ShowShipFromInfo; } }

    public static IEnumerable<FlatInvoice> BlankFlatInvoice()
    {
      return new List<FlatInvoice>();
    }

    public static IEnumerable<FlatInvoice> FromInvoices(IEnumerable<Invoice> invoices)
    {
      foreach (var invoice in invoices)
      {
        foreach (var detail in invoice.InvoiceDetails)
        {
          yield return new FlatInvoice(invoice, detail);
        }
      }
    }
  }
}
