using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class AvailableShipMethodsProxy : Proxy<IEnumerable<ShipMethodDetail>>
    {
        private static volatile AvailableShipMethodsProxy _instance;
        private static object _syncRoot = new object();
        private ICustomerData _customerData;
        private string _fromWarehouse;
        private string _toWarehouse;

        private AvailableShipMethodsProxy()
        {
        }

        public override string PageName
        {
            get { return "getshipmethods.asp"; }
        }

        public static AvailableShipMethodsProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new AvailableShipMethodsProxy();
                }

                return _instance;
            }
        }

        public IEnumerable<ShipMethodDetail> SubmitRequest(ICustomerData customerData, string toWarehouse,
            string fromWarehouse)
        {
            lock (_syncRoot)
            {
                _customerData = customerData;
                _toWarehouse = toWarehouse;
                _fromWarehouse = fromWarehouse;
                return SubmitRequest();
            }
        }


        protected override IEnumerable<ShipMethodDetail> SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;
            var shipMethods = new List<ShipMethodDetail>();

            xmlResponse = sendWebRequest(xmlRequest);

            var shipMethodRecords = new DataSet();

            AdoUtils.FillDataSetWithAdoXml(shipMethodRecords, xmlResponse);

            foreach (DataRow row in shipMethodRecords.Tables[0].Rows)
                shipMethods.Add(new ShipMethodDetail
                {
                    From = row["PLFROM"].ToString().Trim(),
                    To = row["PLTO"].ToString().Trim(),
                    ShipMethod = row["PLTYPE"].ToString().Trim(),
                    Description = row["PLDESC"].ToString().Trim(),
                    Charge = decimal.Parse(row["PLCHARGE"].ToString().Trim())
                });

            return shipMethods;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);

            writer.WriteStartDocument();
            writer.WriteStartElement("request");
            writer.WriteElementString("custid", _customerData.CustomerID.ToString());
            writer.WriteElementString("company", _customerData.CompanyCode.ToString());
            writer.WriteElementString("fromwhse", _fromWarehouse);
            writer.WriteElementString("towhse", _toWarehouse);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}