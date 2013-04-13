using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ValidValue
    {
        private string labelField;
        public string Label
        {
            get { return labelField; }
            set { labelField = value; }
        }

        private string valueField;
        public string Value
        {
            get { return valueField; }
            set { valueField = value; }
        }
    }
}
 