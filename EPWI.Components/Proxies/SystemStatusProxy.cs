namespace EPWI.Components.Proxies
{
    public class SystemStatusProxy : Proxy<bool>
    {
        private static volatile SystemStatusProxy _instance;
        private static readonly object _syncRoot = new object();

        private SystemStatusProxy()
        {
            

        }
        public static SystemStatusProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new SystemStatusProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "abletoprocessorder.asp"; }
        }

        public bool SubmitRequest(string empty = "")
        {
            lock (_syncRoot)
            {
                var result = sendWebRequest(createRequest());
                return result == "Y";
            }
        }

        protected override bool SubmitRequest()
        {
            var result = sendWebRequest(createRequest());
            return result == "Y";
        }

        protected override string createRequest()
        {
            return string.Empty;
        }
    }
}