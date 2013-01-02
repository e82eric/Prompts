using System;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Prompts.Service.ReportService
{
    [SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices", IsNullable = false)]
    public class ReportingService2005ServerInfoHeader : SoapHeader { }
}