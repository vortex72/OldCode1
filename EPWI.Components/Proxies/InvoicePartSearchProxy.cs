using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class InvoicePartSearchProxy : Proxy<IEnumerable<InvoicePartSearchResult>>
    {
        private static volatile InvoicePartSearchProxy _instance;
        private static readonly object _syncRoot = new object();

        private ICustomerData customerData;
        private string partNumber;

        private InvoicePartSearchProxy()
        {
        }

        public static InvoicePartSearchProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new InvoicePartSearchProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "invoicepart.asp"; }
        }

        public IEnumerable<InvoicePartSearchResult> SubmitRequest(ICustomerData customerData, string partNumber)
        {
            lock (_syncRoot)
            {
                if (partNumber == null)
                    throw new ArgumentNullException("partNumber", "Part number is required");

                this.customerData = customerData;
                this.partNumber = partNumber.ToUpper();
                return SubmitRequest();
            }
        }

        protected override IEnumerable<InvoicePartSearchResult> SubmitRequest()
        {
            var searchResults = new List<InvoicePartSearchResult>();

            var invoices = new DataSet();

            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            AdoUtils.FillDataSetWithAdoXml(invoices, xmlResponse);

            foreach (DataRow row in invoices.Tables[0].Rows)
            {
                var invoice = new InvoicePartSearchResult
                {
                    InvoiceDate =
                        row["ZSYMD"].ToString().Length == 8
                            ? row["ZSYMD"].ToString().AS400DateToDate()
                            : DateTime.MaxValue,
                    InvoiceNumber = row["ZINV"].ToString().Trim(),
                    LineCode = row["ZLINE"].ToString().Trim(),
                    ItemNumber = row["ZITEM"].ToString().Trim(),
                    SizeCode = row["ZSIZE"].ToString().Trim(),
                    Type = row["ZTYPE"].ToString().Trim(),
                    OrderDate = row["ZOYMD"].ToString().AS400DateToDate(),
                    Warehouse = row["ZWHSE"].ToString().Trim(),
                    TotalPrice = decimal.Parse(row["ZPRI$"].ToString()),
                    Quantity = int.Parse(row["ZQTY"].ToString()),
                    Kit = row["ZKIT"].ToString()
                };

                if (row["ZSYMD"].ToString().Length != 8)
                    invoice.ShipmentPending = true;

                searchResults.Add(invoice);
            }

            return searchResults;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);

            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("userid", customerData.UserID.ToString());
            writer.WriteElementString("company", customerData.CompanyCode.ToString());
            writer.WriteElementString("custid", customerData.CustomerID.ToString());
            writer.WriteElementString("part", partNumber);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}