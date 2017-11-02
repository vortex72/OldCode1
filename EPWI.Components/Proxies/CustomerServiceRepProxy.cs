using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class CustomerServiceRepProxy : Proxy<IEnumerable<CustomerServiceRep>>
    {
        private static volatile CustomerServiceRepProxy _instance;
        private static object _syncRoot = new object();
        private ICustomerData customerData;
        private string warehouse;

        private CustomerServiceRepProxy()
        {
        }

        public override string PageName
        {
            get { return "getcsrdata.asp"; }
        }

        public static CustomerServiceRepProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new CustomerServiceRepProxy();
                }

                return _instance;
            }
        }

        public IEnumerable<CustomerServiceRep> SubmitRequest(ICustomerData customerData, string warehouse)
        {
            lock (_syncRoot)
            {
                if (customerData == null)
                    throw new ArgumentNullException("customerData");
                if (warehouse == null)
                    throw new ArgumentNullException("warehouse");

                this.customerData = customerData;
                this.warehouse = warehouse;
                return SubmitRequest();
            }
        }


        protected override IEnumerable<CustomerServiceRep> SubmitRequest()
        {
            var reps = new List<CustomerServiceRep>();
            var xmlRequest = createRequest();
            var xmlResponse = sendWebRequest(xmlRequest);

            var ds = new DataSet();

            AdoUtils.FillDataSetWithAdoXml(ds, xmlResponse);

            foreach (DataRow record in ds.Tables[0].Rows)
                reps.Add(new CustomerServiceRep
                {
                    CustomerID = int.Parse(record["ZEMAUCST"].ToString()),
                    CompanyCode = record["ZEMAUCMP"].ToString()[0],
                    UserCode = record["ZEMAUC"].ToString(),
                    UserName = record["ZEMAUNAME"].ToString().Trim(),
                    JobID = int.Parse(record["ZEMAU2JOB"].ToString()),
                    LocationCode = record["ZEMAULOC"].ToString()
                });

            return reps;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("company", customerData.CompanyCode.ToString());
            writer.WriteElementString("custid", customerData.CustomerID.ToString());
            writer.WriteElementString("whse", warehouse);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}