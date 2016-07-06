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
    public class TradeXML_RFQAdmin
    {
        [XmlElement(ElementName = "Action")]
        public string Action { get; set; }

        [XmlElement(ElementName = "QUOTES")]
        public TradeQuotes_RFQAdmin QuotesList { get; set; }

        public string Xml
        {
            get
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TradeXML_RFQAdmin));
                StringBuilder output = new StringBuilder();
                var writer = new StringWriter(output);

                serializer.Serialize(writer, this);
                return output.ToString();
            }
        }
    }

    [XmlRoot("QUOTES")]
    public class TradeQuotes_RFQAdmin
    {
        [XmlElement(ElementName = "Q")]
        public List<TradeQ_RFQAdmin> Q { get; set; }
    }

    public class TradeQ_RFQAdmin
    {
        [XmlAttribute(AttributeName = "QReqID")]
        public string QReqID { get; set; }
        [XmlAttribute(AttributeName = "EmailId")]
        public string EmailId { get; set; }
    }
}
