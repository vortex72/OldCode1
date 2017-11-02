using System;
using System.Collections.Generic;
using System.Linq;

namespace EPWI.Components.Models
{
  public class InvoiceDateSearch
  {
    public bool ErrorOccurred { get; set; }
    public string ErrorDescription { get; set; }
    public InvoiceSearchDirection SearchDirection { get; set; }
    public DateTime SearchDate { get; set; }

    public ICustomerData CustomerData { get; set; }
    public IEnumerable<InvoiceDateSearchResult> Invoices { get; set; }

    public DateTime FirstDate
    {
      get
      {
        DateTime date = SearchDate;

        if (Invoices.Count() > 0)
        {
          var shipmentDates = (from i in Invoices
                                where !i.ShipmentPending
                                select i.ShipmentDate);
          date = shipmentDates.Count() > 0 ? shipmentDates.Min() : SearchDate;
        }

        return SearchDirection == InvoiceSearchDirection.After ? SearchDate : date;
      }
    }

    public DateTime LastDate
    {
      get
      {
        DateTime date = SearchDate;

        if (Invoices.Count() > 0)
        {
          var shipmentDates = (from i in Invoices
                                where !i.ShipmentPending 
                                select i.ShipmentDate);
          date = shipmentDates.Count() > 0 ? shipmentDates.Max() : SearchDate;
        }

        return SearchDirection == InvoiceSearchDirection.Before ? SearchDate : new DateTime(Math.Max(date.Ticks,SearchDate.Ticks));
      }
    }

  }
}
