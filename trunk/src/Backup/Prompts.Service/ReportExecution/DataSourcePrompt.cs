using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class DataSourcePrompt
    {
        public string Name { get; set; }
        public string DataSourceId { get; set; }
        public string Prompt { get; set; }
    }
}