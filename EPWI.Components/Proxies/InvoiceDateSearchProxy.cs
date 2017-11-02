using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class InvoiceDateSearchProxy : Proxy<InvoiceDateSearch>
    {
        private static volatile InvoiceDateSearchProxy _instance;
        private static readonly object _syncRoot = new object();

        private ICustomerData customerData;
        private DateTime invoiceDate;
        private string searchDirection;

        private InvoiceDateSearchProxy()
        {
        }

        public static InvoiceDateSearchProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new InvoiceDateSearchProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "invoicelist.asp"; }
        }

        public InvoiceDateSearch SubmitRequest(ICustomerData customerData, DateTime invoiceDate, string searchDirection)
        {
            lock (_syncRoot)
            {
                this.customerData = customerData;
                this.invoiceDate = invoiceDate;
                this.searchDirection = searchDirection;
                return SubmitRequest();
            }
        }

        protected override InvoiceDateSearch SubmitRequest()
        {
            var searchResults = new List<InvoiceDateSearchResult>();

            var invoices = new DataSet();

            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            AdoUtils.FillDataSetWithAdoXml(invoices, xmlResponse);

            var result = new InvoiceDateSearch();

            // if error is returned, just grab the two applicable columns
            if (invoices.Tables[0].Columns.Contains("ErrorOccured"))
            {
                result.ErrorOccurred = bool.Parse(invoices.Tables[0].Rows[0]["ErrorOccured"].ToString());
                result.ErrorDescription = invoices.Tables[0].Rows[0]["ErrorDescription"].ToString();
            }
            else

                foreach (DataRow row in invoices.Tables[0].Rows)
                {
                    var invoice = new InvoiceDateSearchResult
                    {
                        InvoiceNumber = row["ZINV"].ToString().Trim(),
                        Type = row["ZTYPE"].ToString().Trim(),
                        ShippingWarehouse = row["ZWHSE"].ToString().Trim(),
                        InvoiceTotal = decimal.Parse(row["ZINV$"].ToString()),
                        LineItemCount = int.Parse(row["Z#LI"].ToString()),
                        ShipmentMethod = row["ZSHP"].ToString().Trim(),
                        SoldToAccount = row["ZOEC#"].ToString().Trim(),
                        PONumber = row["ZCPO"].ToString().Trim(),
                        StatementDate =
                            row["ZSYM"].ToString().Length == 6
                                ? new DateTime(int.Parse(row["ZSYM"].ToString().Substring(0, 4)),
                                    int.Parse(row["ZSYM"].ToString().Substring(4, 2)), 1)
                                : DateTime.MaxValue,
                        OrderDate = row["ZOYMD"].ToString().AS400DateToDate()
                    };

                    if (row["ZSYMD"].ToString() == "99999999")
                    {
                        invoice.ShipmentPending = true;
                        invoice.ShipmentDate = DateTime.MaxValue;
                    }
                    else
                    {
                        invoice.ShipmentDate = row["ZSYMD"].ToString().AS400DateToDate();
                    }

                    searchResults.Add(invoice);
                }

            result.Invoices = searchResults;

            return result;
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
            writer.WriteElementString("invdate", invoiceDate.ToString("yyyyMMdd"));
            writer.WriteElementString("direction", searchDirection);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}