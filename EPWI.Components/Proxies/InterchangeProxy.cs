using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using ADODB;

namespace EPWI.Components.Proxies
{
    public class InterchangeProxy : Proxy<DataTable>
    {
        private static volatile InterchangeProxy _instance;
        private static object _syncRoot = new object();
        private string companyCode;
        private int customerId;
        private int cylinders;
        private int nipcCode;
        private int quantityRequested;
        private string size;


        private InterchangeProxy()
        {
        }

        public static InterchangeProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new InterchangeProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "interchange.asp"; }
        }

        public DataTable SubmitRequest(char? companyCode, int? customerId, int nipcCode, string size,
            int quantityRequested,
            int cylinders)
        {
            lock (_syncRoot)
            {
                if (!companyCode.HasValue)
                    throw new ArgumentNullException("companyCode is required");

                if (!customerId.HasValue)
                    throw new ArgumentNullException("customerId is required");

                this.companyCode = companyCode.Value.ToString();
                this.customerId = customerId.Value;
                this.nipcCode = nipcCode;
                this.size = size == "STD" ? string.Empty : size;
                this.quantityRequested = quantityRequested;
                this.cylinders = cylinders;
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
            writer.WriteElementString("nicode", nipcCode.ToString());
            writer.WriteElementString("size", size);
            writer.WriteElementString("quantity", quantityRequested.ToString());
            writer.WriteElementString("cylinders", cylinders.ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}