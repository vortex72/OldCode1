using System;
using System.Text;
using System.Xml;
using EPWI.Components.Models;

namespace EPWI.Components.Proxies
{
    public class EmailCustomerServiceRepProxy : Proxy<bool>
    {
        private static volatile EmailCustomerServiceRepProxy _instance;
        private static readonly object _syncRoot = new object();
        private string csr;
        private ICustomerData customerData;
        private readonly int MAXIMUM_MESSAGE_LENGTH = 512;
        private string message;
        private int quoteID;


        private EmailCustomerServiceRepProxy()
        {
        }

        public static EmailCustomerServiceRepProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new EmailCustomerServiceRepProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "emailcsr.asp"; }
        }

        public bool SubmitRequest(ICustomerData customerData, int quoteID, string csr, string message)
        {
            lock (_syncRoot)
            {
                if (message == null)
                    throw new ArgumentNullException("message");
                if (message.Length > MAXIMUM_MESSAGE_LENGTH)
                    throw new ArgumentException($"Message cannot be longer than {MAXIMUM_MESSAGE_LENGTH} characters",
                        "message");
                this.customerData = customerData;
                this.quoteID = quoteID;
                this.csr = csr;
                this.message = message;
                return SubmitRequest();
            }
        }

        protected override bool SubmitRequest()
        {
            var response = sendWebRequest(createRequest());

            return response == "SUCCESS\r\n";
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);

            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("company", customerData.CompanyCode.ToString());
            writer.WriteElementString("custid", customerData.CustomerID.ToString());
            writer.WriteElementString("quote", quoteID.ToString());
            writer.WriteElementString("csr", csr);
            writer.WriteElementString("msg", message);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}