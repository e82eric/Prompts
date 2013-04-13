using System;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [SerializableAttribute]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    [XmlRootAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices", IsNullable = false)]
    public class ReportExecution2005TrustedUserHeader : SoapHeader
    {
        public string UserName { get; set; }

        [XmlElementAttribute(DataType = "base64Binary")]
        public byte[] UserToken { get; set; }

        [XmlAnyAttributeAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}