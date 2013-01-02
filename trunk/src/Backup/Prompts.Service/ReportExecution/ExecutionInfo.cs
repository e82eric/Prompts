using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [XmlInclude(typeof(ExecutionInfo2))]
    [Serializable]
    [XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ExecutionInfo
    {
        public bool HasSnapshot { get; set; }
        public bool NeedsProcessing { get; set; }
        public bool AllowQueryExecution { get; set; }
        public bool CredentialsRequired { get; set; }
        public bool ParametersRequired { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public DateTime ExecutionDateTime { get; set; }
        public int NumPages { get; set; }
        public ReportParameter[] Parameters { get; set; }
        public DataSourcePrompt[] DataSourcePrompts { get; set; }
        public bool HasDocumentMap { get; set; }
        public string ExecutionID { get; set; }
        public string ReportPath { get; set; }
        public string HistoryID { get; set; }
        public PageSettings ReportPageSettings { get; set; }
        public int AutoRefreshInterval { get; set; }
    }
}