using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class OrderItemUpdateProxy : Proxy<bool>
    {
        private static volatile OrderItemUpdateProxy _instance;
        private static readonly object _syncRoot = new object();

        private Order order;
        private bool perpetualQuote;
        private string quoteNumber;
        private bool saveAsQuote;

        private OrderItemUpdateProxy()
        {
        }

        public static OrderItemUpdateProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderItemUpdateProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get
            {
                return
                    $"update_oeitems.asp?quotenumber={quoteNumber ?? "0"}&saveasquote={saveAsQuote}&perpetualquote={perpetualQuote}";
            }
        }

        public bool SubmitRequest(Order order, bool saveAsQuote, bool perpetualQuote, string quoteNumber)
        {
            lock (_syncRoot)
            {
                this.order = order;
                this.saveAsQuote = saveAsQuote;
                this.perpetualQuote = perpetualQuote;
                this.quoteNumber = quoteNumber;
                return SubmitRequest();
            }
        }

        protected override bool SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            var xDoc = XDocument.Load(new StringReader(xmlResponse));
            var success = (xDoc.Element("orderresponse").Element("orderprocessing") != null) &&
                          (xDoc.Element("orderresponse").Element("orderprocessing").Element("result").Value == "SUCCESS");

            return success;
        }

        protected override string createRequest()
        {
            // project orders onto an anonymous type with all the data the host expects. Coalesce columns for which the host requires a value.
            var orderItemsForHost = (from item in order.OrderItems
                select new
                {
                    item.OrderItemID,
                    item.OrderID,
                    item.SequenceNumber,
                    item.ItemNumber,
                    item.LineCode,
                    item.NIPCCode,
                    item.Quantity,
                    SizeCode = item.SizeCode ?? string.Empty,
                    item.Warehouse,
                    ShipMethod = item.ShipMethod ?? string.Empty,
                    item.Price,
                    item.PricingPercentage,
                    DiscountPercent = item.DiscountPercent.GetValueOrDefault(0),
                    KitComponent = item.KitComponent == null ? " " : item.KitComponent.ToString(),
                    CoreItem = item.CoreItem == null ? " " : item.CoreItem.ToString(),
                    item.KitSelectionMethod,
                    item.OrderMethod,
                    InterchangeMethod = item.InterchangeMethod == null ? " " : item.InterchangeMethod.ToString(),
                    item.ShipVia,
                    item.ErrorFlag,
                    item.ZeroPrice,
                    ParentItemID = item.ParentItemID.GetValueOrDefault(0),
                    KitData = item.KitData ?? string.Empty,
                    Notes = item.Notes ?? string.Empty,
                    item.DateAdded,
                    item.Order.EPWCustID,
                    item.Order.EPWCompCode,
                    item.Order.UserID,
                    item.ItemDescription,
                    item.CustomerReference
                }).OrderBy(oi => oi.SequenceNumber).ToList();

            var orderItemsDataTable = DataSetLinqOperators.CopyToDataTable(orderItemsForHost);

            var ds = new DataSet();
            ds.Tables.Add(orderItemsDataTable);

            return AdoUtils.GetADORecordSetXml(ds);
        }
    }
}