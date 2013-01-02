using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ReportParameter
    {
        public string Name { get; set; }

        public ParameterTypeEnum Type { get; set; }

        [XmlIgnoreAttribute]
        public bool TypeSpecified { get; set; }

        public bool Nullable { get; set; }

        [XmlIgnoreAttribute]
        public bool NullableSpecified { get; set; }

        public bool AllowBlank { get; set; }

        [XmlIgnoreAttribute]
        public bool AllowBlankSpecified { get; set; }

        public bool MultiValue { get; set; }

        [XmlIgnoreAttribute]
        public bool MultiValueSpecified { get; set; }

        public bool QueryParameter { get; set; }

        [XmlIgnoreAttribute]
        public bool QueryParameterSpecified { get; set; }

        public string Prompt { get; set; }

        public bool PromptUser { get; set; }

        [XmlIgnoreAttribute]
        public bool PromptUserSpecified { get; set; }

        [XmlArrayItemAttribute("Dependency")]
        public string[] Dependencies { get; set; }

        public bool ValidValuesQueryBased { get; set; }

        [XmlIgnoreAttribute]
        public bool ValidValuesQueryBasedSpecified { get; set; }

        public ValidValue[] ValidValues { get; set; }

        public bool DefaultValuesQueryBased { get; set; }

        [XmlIgnoreAttribute]
        public bool DefaultValuesQueryBasedSpecified { get; set; }

        [XmlArrayItemAttribute("Value")]
        public string[] DefaultValues { get; set; }

        public ParameterStateEnum State { get; set; }

        [XmlIgnoreAttribute]
        public bool StateSpecified { get; set; }

        public string ErrorMessage { get; set; }
    }
}