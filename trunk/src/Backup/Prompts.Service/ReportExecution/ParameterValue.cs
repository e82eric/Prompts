using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ParameterValue : ParameterValueOrFieldReference
    {

        private string nameField;

        private string valueField;

        private string labelField;

        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }

        public string Label
        {
            get
            {
                return labelField;
            }
            set
            {
                labelField = value;
            }
        }
    }
}