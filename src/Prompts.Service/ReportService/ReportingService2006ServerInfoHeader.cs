using System;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace Prompts.Service.ReportService
{
    [SerializableAttribute]
    [XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2006/03/15/reporting/reportingservices")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/sqlserver/2006/03/15/reporting/reportingservices", IsNullable = false)]
    public class ReportingService2006ServerInfoHeader : SoapHeader { }
}