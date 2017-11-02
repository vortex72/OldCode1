using System;
using System.Text;
using System.Xml;
using log4net;

namespace EPWI.Components.Proxies
{
    public class MillionthPartProxy : Proxy<bool>
    {
        private static readonly ILog log = LogManager.GetLogger("MillionthPartProxy");

        private static volatile MillionthPartProxy _instance;
        private static readonly object _syncRoot = new object();
        private char companyCode;
        private int customerID;
        private DateTime guessDateTime;
        private int orderNumber;
        private string userID;


        private MillionthPartProxy()
        {
        }

        public static MillionthPartProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new MillionthPartProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "millionthpart.asp"; }
        }

        public bool SubmitRequest(DateTime guessDateTime, string userID, int customerID, char companyCode,
            int orderNumber)
        {
            lock (_syncRoot)
            {
                this.guessDateTime = guessDateTime;
                this.userID = userID;
                this.customerID = customerID;
                this.companyCode = companyCode;
                this.orderNumber = orderNumber;
                return SubmitRequest();
            }
        }

        protected override bool SubmitRequest()
        {
            var success = true;
            var request = createRequest();
            var result = sendWebRequest(request);

            if (result != "SUCCESS\r\n")
            {
                success = false;
                log.ErrorFormat("Error submitting millionth part guess. Response: {0}", result);
            }

            return success;
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            writer.WriteStartElement("guess");
            writer.WriteElementString("company", companyCode.ToString());
            writer.WriteElementString("custid", customerID.ToString());
            writer.WriteElementString("ordernumber", orderNumber.ToString());
            writer.WriteElementString("username", userID);
            writer.WriteElementString("guessdate", guessDateTime.ToString("yyyyMMdd"));
            writer.WriteElementString("guesstime", guessDateTime.ToString("HHmm00"));
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}