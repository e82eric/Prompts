using System;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    [XmlRootAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices", IsNullable = false)]
    public class ExecutionHeader : SoapHeader
    {
        public string ExecutionID { get; set; }
        [XmlAnyAttributeAttribute]
        public XmlAttribute[] AnyAttr { get; set; }
    }
}