namespace EPWI.Components.Proxies
{
    public class AcesKitNumberProxy : Proxy<string>
    {
        private static volatile AcesKitNumberProxy _instance;
        private static object _syncRoot = new object();
        private string baseKitNumber;


        private AcesKitNumberProxy()
        {
        }

        public static AcesKitNumberProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new AcesKitNumberProxy();
                }

                return _instance;
            }
        }

        public override string PageName => $"aces_counter.asp?kit={baseKitNumber}";


        public string SubmitRequest(string baseKitNumber)
        {
            lock (_syncRoot)
            {
                this.baseKitNumber = baseKitNumber;
                return SubmitRequest();
            }
        }

        protected override string SubmitRequest()
        {
            return sendWebRequest(createRequest());
        }

        protected override string createRequest()
        {
            // no data needed here. Data required is in query string
            return string.Empty;
        }
    }
}