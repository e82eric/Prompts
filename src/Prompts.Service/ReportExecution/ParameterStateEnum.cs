using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [Serializable]
    [XmlType(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public enum ParameterStateEnum
    {
        HasValidValue,
        MissingValidValue,
        HasOutstandingDependencies,
        DynamicValuesUnavailable,
    }
}