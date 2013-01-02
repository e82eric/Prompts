using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportExecution
{
    [XmlInclude(typeof(ParameterValue))]
    [Serializable]
    [XmlTypeAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices")]
    public class ParameterValueOrFieldReference
    {
    }
}