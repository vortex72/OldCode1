using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class InvoiceProxy : Proxy<Invoice>
    {
        private static volatile InvoiceProxy _instance;
        private static readonly object _syncRoot = new object();


        private ICustomerData customerData;
        private string invoiceNumber;

        private InvoiceProxy()
        {
        }

        public static InvoiceProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new InvoiceProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "InvoiceDetail.asp"; }
        }

        public Invoice SubmitRequest(ICustomerData customerData, string invoiceNumber)
        {
            lock (_syncRoot)
            {
                this.customerData = customerData;
                this.invoiceNumber = invoiceNumber;
                return SubmitRequest();
            }
        }

        protected override Invoice SubmitRequest()
        {
            Invoice invoice;
            var headerRecords = new DataSet();
            var detailRecords = new DataSet();

            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            var index = xmlResponse.IndexOf("</xml>") + 5;

            var headerPacket = xmlResponse.Substring(0, index + 1);
            var detailPacket = xmlResponse.Substring(index + 3);

            if ((headerPacket.Substring(0, 4) != "<xml") || (detailPacket.Substring(0, 4) != "<xml"))
            {
                invoice = new Invoice {Error = true};
            }
            else
            {
                AdoUtils.FillDataSetWithAdoXml(headerRecords, headerPacket);
                AdoUtils.FillDataSetWithAdoXml(detailRecords, detailPacket);

                if ((headerRecords.Tables[0].Rows.Count == 0) || (detailRecords.Tables[0].Rows.Count == 0))
                {
                    invoice = new Invoice {Error = true};
                    return invoice;
                }

                var headerRecord = headerRecords.Tables[0].Rows[0];

                invoice = new Invoice
                {
                    CustomerID = int.Parse(headerRecord["OCUST"].ToString()),
                    InvoiceNumber = headerRecord["OICN"].ToString(),
                    Associate = headerRecord["OEOP"].ToString(),
                    SoldToAddress = new Address
                    {
                        Name =
                            $"{headerRecord["OCNAM"].ToString().Trim()} | {headerRecord["OCUST"]} ({customerData.CompanyCode})",
                        StreetAddress1 = headerRecord["OCAD1"].ToString().Trim(),
                        StreetAddress2 = headerRecord["OCAD2"].ToString().Trim(),
                        City = headerRecord["OCCIT"].ToString().Trim(),
                        State = headerRecord["OCST"].ToString().Trim(),
                        Zip = headerRecord["OCZIP"].ToString().Trim(),
                        Zip4 = headerRecord["OCZIP4"].ToString().Trim(),
                        Phone = headerRecord["OCPH"].ToString().Trim()
                    },
                    ShipToAddress = new Address
                    {
                        Name =
                            string.IsNullOrEmpty(headerRecord["OSNAM"].ToString().Trim())
                                ? string.Empty
                                : headerRecord["OSNAM"].ToString().Trim()
                        /* string.Format("{0} | {1} ({2})", headerRecord["OSNAM"].ToString().Trim(), headerRecord["OSCUST"].ToString(), customerData.CompanyCode) */,
                        StreetAddress1 = headerRecord["OSADR"].ToString().Trim(),
                        City = headerRecord["OSCIT"].ToString().Trim(),
                        State = headerRecord["OSST"].ToString().Trim(),
                        Zip = headerRecord["OSZIP"].ToString().Trim(),
                        Zip4 = headerRecord["OSZIP4"].ToString().Trim()
                    },
                    BillToAddress = new Address
                    {
                        Name = string.IsNullOrEmpty(headerRecord["OBNAM"].ToString().Trim())
                            ? string.Empty
                            : $"{headerRecord["OBNAM"].ToString().Trim()} | {headerRecord["OBCUST"]} ({customerData.CompanyCode})",
                        StreetAddress1 = headerRecord["OBAD1"].ToString().Trim(),
                        StreetAddress2 = headerRecord["OBAD2"].ToString().Trim(),
                        City = headerRecord["OBCIT"].ToString().Trim(),
                        State = headerRecord["OBST"].ToString().Trim(),
                        Zip = headerRecord["OBZIP"].ToString().Trim(),
                        Zip4 = headerRecord["OBZIP4"].ToString().Trim(),
                        Phone = headerRecord["OBPH"].ToString().Trim()
                    },
                    CreateDate = headerRecord["OYMDE"].ToString().AS400DateToDate(),
                    ShippingCarrier = headerRecord["OSH"].ToString().Trim(),
                    TrackingNumbers = getItemList(headerRecord, "OZTK", 10),
                    CustomerNotes = getItemList(headerRecord, "CNTE", 10),
                    ShippingWarehouse = headerRecord["OWH"].ToString(),
                    Terms = headerRecord["OTERM"].ToString(),
                    PONumber = headerRecord["OCPO"].ToString().Trim(),
                    OrderNotes1 = headerRecord["OCOM1"].ToString().Trim(),
                    OrderNotes2 = headerRecord["OCOM2"].ToString().Trim(),
                    MerchandiseTotal = decimal.Parse(headerRecord["OT$S"].ToString()),
                    InvoiceTotal = decimal.Parse(headerRecord["O$ICN"].ToString()),
                    TotalDue = decimal.Parse(headerRecord["OT$CL"].ToString()),
                    CompanyCode = headerRecord["ZCOMP"].ToString(),
                    CustomerWarehouse = headerRecord["OCWH"].ToString(),
                    Type = headerRecord["OTYPE"].ToString()
                };

                if (headerRecord["OYMDS"].ToString() == "99999999")
                {
                    invoice.ShipmentPending = true;
                    invoice.ShipDate = DateTime.MaxValue;
                }
                else
                {
                    invoice.ShipDate = headerRecord["OYMDS"].ToString().AS400DateToDate();
                }

                var details = new List<InvoiceDetail>();

                foreach (DataRow row in detailRecords.Tables[0].Rows)
                    details.Add(new InvoiceDetail
                    {
                        SequenceNumber = int.Parse(row["OISEQ"].ToString()),
                        KitComponent = row["OIKIT"].ToString(),
                        QuantityOrdered = int.Parse(row["OIQO"].ToString()),
                        QuantityShipped = int.Parse(row["OIQS"].ToString()),
                        LineCode = row["OILIN"].ToString(),
                        ItemNumber = row["OIITM"].ToString().Trim(),
                        SizeCode = row["OISIZ"].ToString().Trim(),
                        Description = row["OIDES"].ToString().Trim(),
                        Warehouse = row["OWH"].ToString(),
                        PONumber = row["OICPO"].ToString().Trim(),
                        NetPrice = decimal.Parse(row["OINET"].ToString()),
                        Notes = row["OICOM"].ToString().Trim(),
                        ShipFromCompany = row["OIFCO"].ToString().Trim(),
                        ShipFromWarehouse = row["OIFLO"].ToString().Trim()
                    });
                invoice.InvoiceDetails = details;
            }

            return invoice;
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
            writer.WriteElementString("invoice", invoiceNumber);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}