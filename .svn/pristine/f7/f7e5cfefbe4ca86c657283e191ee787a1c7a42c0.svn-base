using EPWI.Components.Models;

namespace EPWI.Components.Proxies
{
    public class CustomerNumberByInvoiceProxy : Proxy<CustomerNumberByInvoiceResult>
    {
        private static volatile CustomerNumberByInvoiceProxy _instance;
        private static object _syncRoot = new object();
        private string _invoiceNumber;

        private CustomerNumberByInvoiceProxy()
        {
        }

        public override string PageName
        {
            get { return "getinvoicecustomerdata.asp?invoice=" + _invoiceNumber; }
        }

        public static CustomerNumberByInvoiceProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new CustomerNumberByInvoiceProxy();
                }

                return _instance;
            }
        }

        public CustomerNumberByInvoiceResult SubmitRequest(string invoiceNumber)
        {
            lock (_syncRoot)
            {
                _invoiceNumber = invoiceNumber;
                return SubmitRequest();
            }
        }

        protected override CustomerNumberByInvoiceResult SubmitRequest()
        {
            var response = sendWebRequest(createRequest());
            var tokens = response.Split('|');
            return new CustomerNumberByInvoiceResult {CompanyCode = tokens[0][0], CustomerID = int.Parse(tokens[1])};
        }

        protected override string createRequest()
        {
            return string.Empty;
        }
    }
}