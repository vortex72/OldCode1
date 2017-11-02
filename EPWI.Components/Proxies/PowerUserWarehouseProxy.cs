using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using EPWI.Components.Models;

namespace EPWI.Components.Proxies
{
    public class PowerUserWarehouseProxy : Proxy<PowerUserWarehouseResult>
    {
        private static volatile PowerUserWarehouseProxy _instance;
        private static readonly object _syncRoot = new object();

        private ICustomerData CustomerData;
        private Address ShipToAddress;

        private PowerUserWarehouseProxy()
        {
        }

        public override string PageName
        {
            get { return "GetShippingWarehouses.asp"; }
        }

        public static PowerUserWarehouseProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new PowerUserWarehouseProxy();
                }

                return _instance;
            }
        }

        public PowerUserWarehouseResult SubmitRequest(ICustomerData customerData, Address shipToAddress)
        {
            lock (_syncRoot)
            {
                CustomerData = customerData;
                ShipToAddress = shipToAddress;
                return SubmitRequest();
            }
        }


        protected override PowerUserWarehouseResult SubmitRequest()
        {
            if ((CustomerData == null) || (ShipToAddress == null))
                throw new InvalidOperationException("CustomerData and ShipToAddress must be specified");
            var xmlRequest = createRequest();

            var xmlResponse = sendWebRequest(xmlRequest);

            var xDoc = XDocument.Load(new StringReader(xmlResponse));

            var results = xDoc.Element("shippingresponse").Elements();

            var primaryWarehouse = results.First(x => x.Name.LocalName == "primarywhse").Value;
            var secondaryWarehouse = results.First(x => x.Name.LocalName == "secondarywhse").Value;
            var assignedWarehouse = results.First(x => x.Name.LocalName == "assignedwhse").Value;

            return new PowerUserWarehouseResult
            {
                PrimaryWarehouse = primaryWarehouse,
                SecondaryWarehouse = secondaryWarehouse,
                AssignedWarehouse = assignedWarehouse
            };
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);

            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("company", CustomerData.CompanyCode.ToString());
            writer.WriteElementString("custid", CustomerData.CustomerID.ToString());
            writer.WriteElementString("shiptoaddress", ShipToAddress.StreetAddress1);
            writer.WriteElementString("shiptocity", ShipToAddress.City);
            writer.WriteElementString("shiptostate", ShipToAddress.State);
            writer.WriteElementString("shiptozip", ShipToAddress.Zip);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}