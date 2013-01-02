using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ReportMargins
    {
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
    }
}