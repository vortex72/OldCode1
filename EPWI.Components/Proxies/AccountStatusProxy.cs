using System;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class AccountStatusProxy : Proxy<AccountStatus>
    {
        private static volatile AccountStatusProxy _instance;
        private static object _syncRoot = new object();
        private ICustomerData customerData;

        private AccountStatusProxy()
        {
        }

        public static AccountStatusProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new AccountStatusProxy();
                }

                return _instance;
            }
        }

        public override string PageName => "accountstatus.asp";

        public AccountStatus SubmitRequest(ICustomerData data)
        {
            lock (_syncRoot)
            {
                customerData = data;
                return SubmitRequest();
            }
        }

        protected override AccountStatus SubmitRequest()
        {
            var accountStatus = new AccountStatus();
            var xmlRequest = createRequest();
            var xmlResponse = sendWebRequest(xmlRequest);

            var ds = new DataSet();

            AdoUtils.FillDataSetWithAdoXml(ds, xmlResponse);

            var row = ds.Tables[0].Rows[0];

            // if error is returned, just grab the two applicable columns
            if (ds.Tables[0].Columns.Contains("ErrorOccured"))
            {
                accountStatus.ErrorOccurred = bool.Parse(row["ErrorOccured"].ToString());
                accountStatus.ErrorDescription = row["ErrorDescription"].ToString();
            }
            else
            {
                var date = row["ZYMD"].ToString();
                accountStatus.Date = date.AS400DateToDate();
                var lastStatementDate = row["ZSYM"].ToString();
                accountStatus.LastStatementDate = new DateTime(int.Parse(lastStatementDate.Substring(0, 4)),
                    int.Parse(lastStatementDate.Substring(4, 2)), 1);
                accountStatus.CustomerID = int.Parse(row["ZCUST"].ToString());
                accountStatus.CompanyCode = row["ZCOMP"].ToString();
                accountStatus.CurrentMonthSales = decimal.Parse(row["ZSLS"].ToString());
                accountStatus.PastDueCurrentMonth = decimal.Parse(row["ZCDU1"].ToString());
                accountStatus.PastDueLastMonth = decimal.Parse(row["ZCDU2"].ToString());
                accountStatus.PastDue30 = decimal.Parse(row["ZPD30"].ToString());
                accountStatus.PastDue60 = decimal.Parse(row["ZPD60"].ToString());
                accountStatus.PastDue90 = decimal.Parse(row["ZPD90"].ToString());
                accountStatus.LastMonthSales = decimal.Parse(row["ZLSLS"].ToString());
                accountStatus.CODCustomer = row["ZCOD"].ToString() == "Y";
                accountStatus.MailStatementToCustomer = row["ZSTFL"].ToString() == "Y";
                accountStatus.StatementBalance = decimal.Parse(row["ZSTMT"].ToString());
                accountStatus.StatementRemainingBalance = decimal.Parse(row["ZSTBA"].ToString());
                accountStatus.DiscountAmount = decimal.Parse(row["ZDIAM"].ToString());
                accountStatus.DiscountPercent = decimal.Parse(row["ZDIPC"].ToString());
                accountStatus.TotalAccountBalance = decimal.Parse(row["ZBAL"].ToString());
                accountStatus.CurrentInvoices = decimal.Parse(row["ZINV"].ToString());
                accountStatus.CurrentCredits = decimal.Parse(row["ZCM"].ToString());
                accountStatus.CurrentAdjustments = decimal.Parse(row["ZADJ"].ToString());
                accountStatus.CurrentNSFChecks = decimal.Parse(row["ZNSF"].ToString());
                accountStatus.CurrentNSFCharges = decimal.Parse(row["ZNSFC"].ToString());
                accountStatus.CurrentMiscellaenous = decimal.Parse(row["ZMISC"].ToString());
                accountStatus.CurrentPaymentsMade = decimal.Parse(row["ZPAY"].ToString());
                accountStatus.CurrentDiscountsAllowed = decimal.Parse(row["ZDISC"].ToString());
                accountStatus.PaymentsAndCreditsApplied = decimal.Parse(row["ZSTREC"].ToString());
            }

            return accountStatus;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);

            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("company", customerData.CompanyCode.ToString());
            writer.WriteElementString("custid", customerData.CustomerID.ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}