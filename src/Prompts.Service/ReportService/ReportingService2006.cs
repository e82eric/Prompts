using System.Net;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using Prompts.Service.ReportCatalogService;

namespace Prompts.Service.ReportService
{
    [WebServiceBinding(Name = "ReportingService2006Soap")]
    class ReportingService2006 : SoapHttpClientProtocol
    {
        public ReportingService2006(string url, ICredentials credentials)
        {
            Url = url;
            Credentials = credentials;
        }

        public ReportingService2006ServerInfoHeader ServerInfoHeaderValue { get; set; }

        [SoapHeaderAttribute("ServerInfoHeaderValue", Direction = SoapHeaderDirection.Out)]
        [SoapDocumentMethodAttribute(
            "http://schemas.microsoft.com/sqlserver/2006/03/15/reporting/reportingservices/ListChildren",
            RequestNamespace = "http://schemas.microsoft.com/sqlserver/2006/03/15/reporting/reportingservices",
            ResponseNamespace = "http://schemas.microsoft.com/sqlserver/2006/03/15/reporting/reportingservices",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlArray("CatalogItems")]
        public CatalogItem[] ListChildren(string Item)
        {
            return (CatalogItem[])Invoke("ListChildren", new object[] { Item })[0];
        }
    }

    class ReportingService2006Compatibility : IReportingService2005
    {
        private readonly ReportingService2006 _integratedProxy;

        public ReportingService2006Compatibility(string url, ICredentials credentials)
        {
            _integratedProxy = new ReportingService2006(url, credentials);
        }

        public CatalogItem[] ListChildren(string Item, bool Recursive)
        {
            return _integratedProxy.ListChildren(Item);
        }
    }
}