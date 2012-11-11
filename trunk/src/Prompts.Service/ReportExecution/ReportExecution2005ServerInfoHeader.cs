using System;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices", IsNullable = false)]
    public class ReportingService2005ServerInfoHeader : SoapHeader
    {
        public string ReportServerVersionNumber { get; set; }
        public string ReportServerEdition { get; set; }
        public string ReportServerVersion { get; set; }
        public string ReportServerDateTime { get; set; }
        [XmlAnyAttributeAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}