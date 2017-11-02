using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using ADODB;

namespace EPWI.Components.Proxies
{
    public class OrderHeaderProxy : Proxy<DataTable>
    {
        private static volatile OrderHeaderProxy _instance;
        private static object _syncRoot = new object();
        private string companyCode;
        private int customerId;
        private bool flushData;


        private OrderHeaderProxy()
        {
        }

        public static OrderHeaderProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderHeaderProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "oeheader.asp"; }
        }

        public DataTable SubmitRequest(char? companyCode, int? customerId, bool flushData)
        {
            lock (_syncRoot)
            {
                if (!companyCode.HasValue)
                    throw new ArgumentNullException("companyCode is required");

                if (!customerId.HasValue)
                    throw new ArgumentNullException("customerId is required");

                this.companyCode = companyCode.Value.ToString();
                this.customerId = customerId.Value;
                this.flushData = flushData;
                return SubmitRequest();
            }
        }

        protected override DataTable SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            var adoStream = new Stream();
            adoStream.Open(Type.Missing, 0, StreamOpenOptionsEnum.adOpenStreamUnspecified, null, null);
            adoStream.WriteText(xmlResponse, 0);
            adoStream.Position = 0;

            var rs = new Recordset();
            rs.Open(adoStream, Type.Missing, 0, LockTypeEnum.adLockUnspecified, -1);
            adoStream.Close();

            var adapter = new OleDbDataAdapter();
            var dataSet = new DataSet();

            adapter.Fill(dataSet, rs, "eSynthesis");

            return dataSet.Tables[0];
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            writer.WriteStartElement("customer");
            writer.WriteElementString("company", companyCode);
            writer.WriteElementString("custid", customerId.ToString());
            writer.WriteElementString("flushdata", flushData.ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}