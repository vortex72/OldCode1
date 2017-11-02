using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml.Linq;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class OrderHeaderUpdateProxy : Proxy<bool>
    {
        private static volatile OrderHeaderUpdateProxy _instance;
        private static readonly object _syncRoot = new object();


        private Order order;
        private bool saveAsQuote;

        private OrderHeaderUpdateProxy()
        {
        }

        public static OrderHeaderUpdateProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderHeaderUpdateProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return $"update_oeheader.asp?saveasquote={saveAsQuote}"; }
        }

        public bool SubmitRequest(Order order, bool saveAsQuote)
        {
            lock (_syncRoot)
            {
                this.order = order;
                this.saveAsQuote = saveAsQuote;
                return SubmitRequest();
            }
        }

        protected override bool SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            var xDoc = XDocument.Load(new StringReader(xmlResponse));

            var success = xDoc.Element("orderresponse").Element("headerprocessing").Element("result").Value == "SUCCESS";

            return success;
        }

        protected override string createRequest()
        {
            // copy the order over to a host order object
            // this removes custom properties and the order items from the order so 
            // that we only submit the fields that the host actually needs
            var hostOrder = new HostOrder
            {
                OrderID = order.OrderID,
                InvoiceNumber = order.InvoiceNumber,
                UserID = order.UserID,
                EPWCustID = order.EPWCustID,
                EPWCompCode = order.EPWCompCode,
                BillToCustID = order.BillToCustID,
                SoldToName = order.SoldToName,
                SoldToAddress1 = order.SoldToAddress1,
                SoldToAddress2 = order.SoldToAddress2,
                SoldToCity = order.SoldToCity,
                SoldToState = order.SoldToState,
                SoldToZip = order.SoldToZip,
                SoldToZip4 = order.SoldToZip4,
                SoldToPhone = order.SoldToPhone,
                UserName = order.UserName,
                PORequired = order.PORequired,
                PONumber = order.PONumber,
                TermsCode = order.TermsCode,
                PriceType = order.PriceType,
                CCOnFile = order.CCOnFile,
                Taxable = order.Taxable,
                TaxValue = order.TaxValue,
                TaxPercent = order.TaxPercent,
                SubTotal = order.SubTotal,
                SpecialCharges = order.SpecialCharges,
                OrderTotal = order.OrderTotal,
                ShipToName = order.ShipToName,
                ShipToAddress1 = order.ShipToAddress1 ?? " ",
                ShipToAddress2 = order.ShipToAddress2 ?? " ",
                ShipToCity = order.ShipToCity ?? " ",
                ShipToState = order.ShipToState ?? " ",
                ShipToZip = order.ShipToZip ?? " ",
                ShipToZip4 = order.ShipToZip4 ?? " ",
                ShipToPhone = order.ShipToPhone ?? " ",
                AllowDropShip = order.AllowDropShip,
                AssignedWhse = order.AssignedWhse,
                PreferredWhse = order.PreferredWhse,
                DefaultShipMethod = order.DefaultShipMethod,
                DefaultShipMethodName = order.DefaultShipMethodName,
                RequestedShipMethod = order.RequestedShipMethod,
                Comments1 = order.Comments1,
                Comments2 = order.Comments2,
                AltContact1 = order.AltContact1,
                AltContact2 = order.AltContact2,
                OrderStatus = order.OrderStatus,
                StatusFlag1 = order.StatusFlag1,
                StatusFlag2 = order.StatusFlag2,
                StatusFlag3 = order.StatusFlag3,
                ManualHandling = order.ManualHandling,
                ManualReason1 = order.ManualReason1,
                ManualReason2 = order.ManualReason2,
                ManualReason3 = order.ManualReason3,
                ManualReason4 = order.ManualReason4,
                CreateDate = order.CreateDate,
                EnteredDate = order.EnteredDate,
                SendEmail = order.SendEmail,
                QuoteNumber = order.QuoteNumber
            };

            // add the single order to a list so we can call CopyToDataTable on it
            var orderList = new List<HostOrder> {hostOrder};

            var orderDataTable = DataSetLinqOperators.CopyToDataTable(orderList);

            var ds = new DataSet();
            ds.Tables.Add(orderDataTable);

            return AdoUtils.GetADORecordSetXml(ds);
        }
    }
}