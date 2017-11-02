using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using EPWI.Components.Models;

namespace EPWI.Components.Proxies
{
    public class AcesProxy : Proxy<bool>
    {
        private static volatile AcesProxy _instance;
        private static object _syncRoot = new object();
        private int _baseKitNipc;
        private string _newKitNumber;
        private string _notes1;
        private string _notes2;
        private string _originalKitNumber;
        private IEnumerable<KitPart> _selectedParts;
        private string _userName;

        private AcesProxy()
        {
        }

        public override string PageName
        {
            get { return "process_aces.asp"; }
        }

        public static AcesProxy Instance
        {
            get
            {
                if (_instance != null) return _instance;
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new AcesProxy();
                }

                return _instance;
            }
        }


        public bool SubmitRequest(string originalKitNumber, string newKitNumber, int baseKitNipc,
            IEnumerable<KitPart> selectedParts, string notes, string userName)
        {
            lock (_syncRoot)
            {
                _originalKitNumber = originalKitNumber;
                _newKitNumber = newKitNumber;
                _selectedParts = selectedParts;
                _baseKitNipc = baseKitNipc;

                if (notes.Length > 40)
                {
                    _notes1 = notes.Substring(0, 40);
                    _notes2 = notes.Substring(40, Math.Min(40, notes.Length - 40));
                }
                else
                {
                    _notes1 = notes;
                }

                _userName = userName;
                return SubmitRequest();
            }
        }

        protected override bool SubmitRequest()
        {
            var xmlRequest = createRequest();
            var xmlResponse = string.Empty;

            xmlResponse = sendWebRequest(xmlRequest);

            return int.Parse(xmlResponse) == _selectedParts.Count();
        }

        protected override string createRequest()
        {
            var xmlRequest = new StringBuilder();
            var writer = XmlWriter.Create(xmlRequest);
            writer.WriteStartDocument();
            writer.WriteStartElement("aceskit");
            writer.WriteElementString("originalkitnumber", _originalKitNumber);
            writer.WriteElementString("newkitnumber", _newKitNumber);
            writer.WriteElementString("basekitnipc", _baseKitNipc.ToString());
            writer.WriteElementString("username", _userName);
            writer.WriteElementString("notes1", _notes1);
            writer.WriteElementString("notes2", _notes2);
            writer.WriteStartElement("parts");
            foreach (var part in _selectedParts)
            {
                writer.WriteStartElement("part");
                writer.WriteAttributeString("sequencenumber", part.SequenceNumber.ToString());
                writer.WriteString(part.NIPCCode.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();

            return xmlRequest.ToString();
        }
    }
}