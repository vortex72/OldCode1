using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class StatementProxy : Proxy<Statement>
    {
        private static volatile StatementProxy _instance;
        private static object _syncRoot = new object();
        private ICustomerData _customerData;
        private DateTime _statementDate;

        private StatementProxy()
        {
        }


        public static StatementProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new StatementProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "statement.asp"; }
        }

        public Statement SubmitRequest(ICustomerData customerData, DateTime statementDate)
        {
            lock (_syncRoot)
            {
                _customerData = customerData;
                _statementDate = statementDate;
                return SubmitRequest();
            }
        }

        protected override Statement SubmitRequest()
        {
            Statement statement;
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
                statement = new Statement {Error = true};
            }
            else
            {
                AdoUtils.FillDataSetWithAdoXml(headerRecords, headerPacket);
                AdoUtils.FillDataSetWithAdoXml(detailRecords, detailPacket);

                if (headerRecords.Tables[0].Rows.Count == 0)
                {
                    statement = new Statement {Error = true};
                    return statement;
                }

                var headerRecord = headerRecords.Tables[0].Rows[0];

                statement = new Statement
                {
                    CustomerID = int.Parse(headerRecord["ZCUST"].ToString()),
                    TotalDue = decimal.Parse(headerRecord["ZSTOT"].ToString()),
                    CurrentBalance = decimal.Parse(headerRecord["ZSCUR"].ToString()),
                    PastDue30 = decimal.Parse(headerRecord["ZSP30"].ToString()),
                    PastDue60 = decimal.Parse(headerRecord["ZSP60"].ToString()),
                    PastDue90 = decimal.Parse(headerRecord["ZSP90"].ToString()),
                    PastDue120 = decimal.Parse(headerRecord["ZSP120"].ToString()),
                    PreviousBalance = decimal.Parse(headerRecord["ZBBAL"].ToString()),
                    StatementDate =
                        new DateTime(int.Parse(headerRecord["ZSYM"].ToString().Substring(0, 4)),
                            int.Parse(headerRecord["ZSYM"].ToString().Substring(4, 2)), 1),
                    CompanyCode = headerRecord["ZCOMP"].ToString().Trim(),
                    Name = headerRecord["NAME"].ToString().Trim(),
                    CustomerNotes = getItemList(headerRecord, "CNTE", 10),
                    CustomerAddress = new Address
                    {
                        StreetAddress1 = headerRecord["ADDR1"].ToString().Trim(),
                        StreetAddress2 = headerRecord["ADDR2"].ToString().Trim(),
                        City = headerRecord["CITY"].ToString().Trim(),
                        State = headerRecord["STATE"].ToString().Trim(),
                        Zip = headerRecord["ZIP"].ToString().Trim(),
                        Zip4 = headerRecord["ZIP4"].ToString().Trim()
                    }
                };


                var details = new List<StatementDetail>();
                detailRecords.Tables[0].DefaultView.Sort = "ZARSEQ";

                foreach (DataRow row in detailRecords.Tables[0].DefaultView.Table.Rows)
                    details.Add(new StatementDetail
                    {
                        TransactionDate = row["ZARYMD"].ToString().AS400DateToDate(),
                        TransactionType = row["ZARTYP"].ToString(),
                        TransactionTypeDescription = row["ZARTYD"].ToString(),
                        ReferenceNumber = row["ZAREF"].ToString(),
                        TermsCode = row["ZTRM"].ToString(),
                        Note = row["ZSNOTE"].ToString(),
                        TransactionAmount = decimal.Parse(row["ZARDAA"].ToString())
                    });

                statement.StatementDetails = details;
            }

            return statement;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);

            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("company", _customerData.CompanyCode.ToString());
            writer.WriteElementString("custid", _customerData.CustomerID.ToString());
            writer.WriteElementString("statementdate", _statementDate.ToString("yyyyMM"));
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}