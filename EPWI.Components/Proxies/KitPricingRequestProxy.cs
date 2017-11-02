using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using EPWI.Components.Models;

namespace EPWI.Components.Proxies
{
    public class KitPricingRequestProxy : Proxy<Kit>
    {
        private static volatile KitPricingRequestProxy _instance;
        private static readonly object _syncRoot = new object();

        private ICustomerData _customerData;
        private Kit _kit;

        private string PrimaryWarehouse;

        private KitPricingRequestProxy()
        {
        }

        public static KitPricingRequestProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new KitPricingRequestProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "kitpricing2.asp"; }
        }

        public Kit SubmitRequest(Kit kit, ICustomerData customerData, string primaryWarehouse = "")
        {
            lock (_syncRoot)
            {
                this._kit = kit;
                this._customerData = customerData;
                PrimaryWarehouse = primaryWarehouse;
                return SubmitRequest();
            }
        }

        protected override Kit SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            var xDoc = XDocument.Load(new StringReader(xmlResponse));

            var kitParts = xDoc.Descendants("kitpart");

            foreach (var partXml in kitParts)
            {
                var kitPart = (from kp in _kit.MasterKitParts
                    where (kp.NIPCCode.ToString() == partXml.Element("partni").Value) &&
                          (kp.SequenceNumber.ToString() == partXml.Element("seq").Value)
                    select kp).SingleOrDefault();
                if (kitPart != null)
                {
                    if (!new[] {"I", "F", "K"}.Contains(kitPart.InterchangeMethod))
                        kitPart.Price = decimal.Parse(partXml.Element("price").Value);

                    // assume that if the part is shipping from another warehouse, that the part is available
                    // this might not always hold true, but for our purposes, it will work
                    if ((partXml.Element("availflag").Value == "Y") || !string.IsNullOrEmpty(kitPart.ShipWarehouse))
                        kitPart.IsAvailable = true;

                    if (partXml.Element("hasinterchange").Value == "Y")
                        kitPart.HasInterchange = true;
                }
            }
            return _kit;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            // this is intentionally misspelled! The epwi service expects "kitpricingreqest" (sic)
            writer.WriteStartElement("kitpricingreqest");
            writer.WriteElementString("comp", _customerData.CompanyCode.ToString());
            writer.WriteElementString("cust", _customerData.CustomerID.ToString());
            writer.WriteElementString("pwhse", PrimaryWarehouse);
            writer.WriteElementString("kitni", _kit?.NIPCCode.ToString());

            foreach (var part in _kit.MasterKitParts)
            {
                writer.WriteStartElement("kitpart");
                writer.WriteElementString("partni", part.NIPCCode.ToString());
                writer.WriteElementString("seq", part.SequenceNumber.ToString());
                writer.WriteElementString("size", part.SizeCode);
                writer.WriteElementString("qty", part.QuantityRequired.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}