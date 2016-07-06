using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Beast_RFQ_Addin
{
    [Serializable]
    [XmlRoot("ExcelInfo")]
    public class SubmitQuoteXML
    {
        [XmlElement(ElementName = "Action")]
        public string Action { get; set; }

        [XmlElement(ElementName = "QUOTES")]
        public Quotes QuotesList { get; set; }

        [XmlElement(ElementName = "Client")]
        public Clients ClientList { get; set; }

        public string Xml
        {
            get
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SubmitQuoteXML));
                StringBuilder output = new StringBuilder();
                var writer = new StringWriter(output);

                serializer.Serialize(writer, this);
                return output.ToString();
            }
        }
    }

    [XmlRoot("QUOTES")]
    public class Quotes
    {
        [XmlElement(ElementName = "Q")]
        public List<Q> Q { get; set; }
    }

    public class Q
    {
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
        [XmlAttribute(AttributeName = "S")]
        public string Side { get; set; }
        [XmlAttribute(AttributeName = "Qty")]
        public int Quantity { get; set; }
        [XmlAttribute(AttributeName = "QReqID")]
        public int RequestId { get; set; }
    }

    [XmlRoot("Client")]
    public class Clients
    {
        [XmlElement(ElementName = "C")]
        public List<C> C { get; set; }
    }

    public class C
    {
        [XmlAttribute(AttributeName = "E")]
        public string EmailId { get; set; }
    }

}
