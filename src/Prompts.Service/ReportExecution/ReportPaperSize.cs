using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ReportPaperSize
    {
        public double Height { get; set; }
        public double Width { get; set; }
    }
}