using System;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using ADODB;

namespace EPWI.Components.Proxies
{
    public class CrunchProxy : Proxy<DataTable>
    {
        private static volatile CrunchProxy _instance;
        private static object _syncRoot = new object();

        private CrunchProxy()
        {
        }

        public static CrunchProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new CrunchProxy();
                }

                return _instance;
            }
        }

        public string PartNumber { get; set; }

        public override string PageName
        {
            get { return "crunch.asp"; }
        }

        public DataTable SubmitRequest(string partNumber)
        {
            lock (_syncRoot)
            {
                PartNumber = partNumber;
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
            writer.WriteStartElement("crunchrequest");
            writer.WriteElementString("part", PartNumber);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}