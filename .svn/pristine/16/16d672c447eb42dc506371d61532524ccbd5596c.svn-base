using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class KitWarrantyProxy : Proxy<List<Warranty>>
    {
        private static volatile KitWarrantyProxy _instance;
        private static readonly object _syncRoot = new object();

        private ICustomerData customerData;
        private int kitNipc;
        private decimal kitPrice;

        private KitWarrantyProxy()
        {
        }

        public static KitWarrantyProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new KitWarrantyProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "warrantypricing.asp"; }
        }

        public List<Warranty> SubmitRequest(int kitNipc, decimal kitPrice, ICustomerData customerData)
        {
            lock (_syncRoot)
            {
                this.kitNipc = kitNipc;
                this.kitPrice = kitPrice;
                this.customerData = customerData;
                return SubmitRequest();
            }
        }

        protected override List<Warranty> SubmitRequest()
        {
            var warranties = new List<Warranty>();
            var xmlRequest = createRequest();
            var xmlResponse = sendWebRequest(xmlRequest);

            var ds = new DataSet();

            AdoUtils.FillDataSetWithAdoXml(ds, xmlResponse);

            foreach (DataRow record in ds.Tables[0].Rows)
                warranties.Add(new Warranty
                {
                    Nipc = int.Parse(record["KWPNI"].ToString()),
                    Price = decimal.Parse(record["KWPRIC"].ToString())
                });

            return warranties;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            writer.WriteStartElement("warrantypricingrequest");
            writer.WriteElementString("comp", customerData.CompanyCode.ToString());
            writer.WriteElementString("cust", customerData.CustomerID.ToString());
            writer.WriteElementString("kitni", kitNipc.ToString());
            writer.WriteElementString("kitprice", kitPrice.ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}