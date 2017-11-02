using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using ADODB;

namespace EPWI.Components.Proxies
{
    public class OrderItemProxy : Proxy<DataTable>
    {
        private static volatile OrderItemProxy _instance;
        private static object _syncRoot = new object();
        private int orderId;

        private OrderItemProxy()
        {
        }

        public static OrderItemProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new OrderItemProxy();
                }

                return _instance;
            }
        }

        public override string PageName
        {
            get { return "oeitems.asp"; }
        }

        public DataTable SubmitRequest(int orderId)
        {
            lock (_syncRoot)
            {
                this.orderId = orderId;
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
            writer.WriteStartElement("order");
            writer.WriteElementString("orderid", orderId.ToString());
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}