using System.Text;
using System.Xml;
using EPWI.Components.Models;
using log4net;

namespace EPWI.Components.Proxies
{
    public class DataDownloadProxy : Proxy<bool>
    {
        private static readonly ILog log = LogManager.GetLogger("EPWI.Components");

        private static volatile DataDownloadProxy _instance;
        private static readonly object _syncRoot = new object();
        private ICustomerData customerData;
        private int format;
        private string[] lines;


        private DataDownloadProxy()
        {
        }

        public static DataDownloadProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new DataDownloadProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "datadownload.asp"; }
        }

        public bool SubmitRequest(ICustomerData customerData, string[] lines, int format)
        {
            lock (_syncRoot)
            {
                this.customerData = customerData;
                this.lines = lines;
                this.format = format;
                return SubmitRequest();
            }
        }

        protected override bool SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            var success = xmlResponse == "Success\r\n";

            if (!success)
                log.ErrorFormat("Error submitting data download request. Response received: {0}", xmlResponse);

            return success;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            writer.WriteStartElement("downloadrequest");
            writer.WriteElementString("company", customerData.CompanyCode.ToString());
            writer.WriteElementString("custid", customerData.CustomerID.ToString());
            writer.WriteElementString("email", customerData.EmailAddress);
            writer.WriteElementString("format", format.ToString());
            writer.WriteStartElement("lines");
            foreach (var line in lines)
            {
                writer.WriteStartElement("line");
                writer.WriteString(line);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}