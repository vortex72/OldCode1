using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using log4net;

namespace EPWI.Components.Proxies
{
    public abstract class Proxy<T>
    {
        protected static ILog log = LogManager.GetLogger("Proxy");

        public string BaseUrl
            => ConfigurationManager.AppSettings["IntegrationServicesBaseUrl"] ?? "http://integration.epwi.net/"
            ;

        public abstract string PageName { get; }

        protected abstract T SubmitRequest();

        protected abstract string createRequest();

        protected string sendWebRequest(string xmlRequest)
        {
            return sendWebRequest(xmlRequest, true);
        }

        protected IEnumerable<string> getItemList(DataRow headerRecord, string fieldPrefix, int itemCount)
        {
            var itemList = new List<string>();

            for (var i = 1; i <= itemCount; i++)
            {
                var item = headerRecord[fieldPrefix + i.ToString("00")].ToString().Trim();

                if (!string.IsNullOrEmpty(item))
                    itemList.Add(item);
            }

            return itemList;
        }

        private string sendWebRequest(string xmlRequest, bool retryOnError)
        {
            // create request
            //xmlRequest = HttpUtility.UrlEncode(xmlRequest);
            var webRequest = WebRequest.Create(BaseUrl + PageName);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            var bytes = Encoding.ASCII.GetBytes(xmlRequest);
            webRequest.ContentLength = bytes.Length;
            var stream = webRequest.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();

            // submit request and get response
            try
            {
                var webResponse = webRequest.GetResponse();
                //TODO: Check status of request
                var responseStream = webResponse.GetResponseStream();
                var reader = new StreamReader(responseStream);
                var xmlResponse = reader.ReadToEnd();
                reader.Close();
                webResponse.Close();

                return xmlResponse;
            }
            catch (WebException ex)
            {
                // retry the request once, due to transient Internal Server Errors from eSynthesis
                if (ex.Message.Contains("Internal Server Error") && retryOnError)
                {
                    log.Warn("Error calling eSynthesis. Resubmitting request.", ex);
                    return sendWebRequest(xmlRequest, false);
                }
                throw;
            }
        }
    }
}