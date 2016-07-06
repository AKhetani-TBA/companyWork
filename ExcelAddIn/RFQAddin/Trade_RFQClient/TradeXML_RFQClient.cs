using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Beast_RFQ_Addin
{
    [Serializable]
    [XmlRoot("ExcelInfo")]
    public class TradeXML_RFQClient
    {
        [XmlElement(ElementName = "Action")]
        public string Action { get; set; }

        [XmlElement(ElementName = "QUOTES")]
        public TradeQuotes_RFQClient QuotesList { get; set; }

        public string Xml
        {
            get
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TradeXML_RFQClient));
                StringBuilder output = new StringBuilder();
                var writer = new StringWriter(output);

                serializer.Serialize(writer, this);
                return output.ToString();
            }
        }
    }

    [XmlRoot("QUOTES")]
    public class TradeQuotes_RFQClient
    {
        [XmlElement(ElementName = "Q")]
        public List<TradeQ_RFQClient> Q { get; set; }
    }

    public class TradeQ_RFQClient
    {
        [XmlAttribute(AttributeName = "QReqID")]
        public string QReqID { get; set; }
        [XmlAttribute(AttributeName = "Price")]
        public string Price { get; set; }
    }
}
