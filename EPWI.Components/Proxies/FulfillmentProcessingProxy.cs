using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using EPWI.Components.Models;
using EPWI.Components.Utility;

namespace EPWI.Components.Proxies
{
    public class FulfillmentProcessingProxy : Proxy<FulfillmentProcessingResult>
    {
        private static volatile FulfillmentProcessingProxy _instance;
        private static readonly object _syncRoot = new object();
        private ICustomerData customerData;
        private Kit kit;

        private string PrimaryWarehouse;
        private string SecondaryWarehouse;


        private FulfillmentProcessingProxy()
        {
        }

        public static FulfillmentProcessingProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new FulfillmentProcessingProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "kitfulfillment2.asp"; }
        }

        public FulfillmentProcessingResult SubmitRequest(Kit kit, ICustomerData customerData, string primaryWarehouse,
            string secondaryWarehouse)
        {
            lock (_syncRoot)
            {
                this.kit = kit;
                this.customerData = customerData;
                PrimaryWarehouse = primaryWarehouse;
                SecondaryWarehouse = secondaryWarehouse;
                return SubmitRequest();
            }
        }

        protected override FulfillmentProcessingResult SubmitRequest()
        {
            var result = new FulfillmentProcessingResult();
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            if (xmlRequest == "N")
            {
                result.ResultCode = "N";
                return result;
            }

            xmlResponse = sendWebRequest(xmlRequest);

            var ds = new DataSet();

            AdoUtils.FillDataSetWithAdoXml(ds, xmlResponse);

            // determine the processing result and users main warehouse by looking at the ZKCOD and ZKWHS1 fields in the first record
            result.ResultCode = ds.Tables[0].Rows[0]["ZKCOD"].ToString().ToUpper();
            result.FulfillmentProcessingRecords = ds.Tables[0];

            return result;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            // this is intentionally misspelled! The epwi service expects "kitfulfillmentreqest" (sic)
            writer.WriteStartElement("kitfulfillmentreqest");
            writer.WriteElementString("user", customerData.UserName);
            writer.WriteElementString("cust", customerData.CustomerID.ToString());
            writer.WriteElementString("comp", customerData.CompanyCode.ToString());
            writer.WriteElementString("kitni", kit.NIPCCode.ToString());

            writer.WriteElementString("pwhse", PrimaryWarehouse ?? "");
            writer.WriteElementString("swhse", SecondaryWarehouse ?? "");

            var partsToFulfill = from kp in kit.MasterKitParts
                where
                kp.Selected &&
                ((kp.IsAvailable == false) || (!string.IsNullOrEmpty(kp.ShipWarehouse) && (kp.ShipWarehouse != "XXX")))
                select kp;

            // if no parts are out of availability or being shipped from an alternate warehouse,
            // then simply exit the function with a return code of "N" for No processing required
            if (partsToFulfill.Count() == 0)
                return "N";

            foreach (var part in partsToFulfill)
            {
                writer.WriteStartElement("kitpart");
                writer.WriteElementString("partni", part.NIPCCode.ToString());
                writer.WriteElementString("seq", part.SequenceNumber.ToString());
                writer.WriteElementString("size", part.SizeCode);
                writer.WriteElementString("qty", part.QuantityRequired.ToString());
                // exception code: tells the AS/400 not to try to interchange the part
                writer.WriteElementString("excode",
                    (part.InterchangeMethod == "I") || (part.InterchangeMethod == "K") ? "Y" : string.Empty);
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