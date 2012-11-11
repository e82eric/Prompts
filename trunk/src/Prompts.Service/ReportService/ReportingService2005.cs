// Copyright (c) 2010. Eric Nelson  
// https://code.google.com/p/prompts/
// All rights reserved.
//     
// Redistribution and use in source and binary forms,   
// with or without modification, are permitted provided   
// that the following conditions are met:    
// * Redistributions of source code must retain the   
// above copyright notice, this list of conditions and   
// the following disclaimer.    
// * Redistributions in binary form must reproduce   
// the above copyright notice, this list of conditions   
// and the following disclaimer in the documentation   
// and/or other materials provided with the distribution.    
// * Neither the name of Eric Nelson nor the   
// names of its contributors may be used to endorse   
// or promote products derived from this software   
// without specific prior written permission.    
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND   
// CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,   
// INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF   
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE   
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR   
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,   
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,   
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR   
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS   
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,   
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING   
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE   
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF   
// SUCH DAMAGE.
// 
//     
// [This is the BSD license, see  
// http://www.opensource.org/licenses/bsd-license.php]  
using System.Net;
using System.Web.Services;
using Prompts.Service.ReportCatalogService;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
namespace Prompts.Service.ReportService
{
    [WebServiceBinding(Name = "ReportingService2005Soap")]
    public class ReportingService2005 : SoapHttpClientProtocol, IReportingService2005
    {
        public ReportingService2005(string url, ICredentials credentials)
        {
            Url = url;
            Credentials = credentials;
        }

        public ReportingService2005ServerInfoHeader ServerInfoHeaderValue { get; set; }

        [SoapHeaderAttribute("ServerInfoHeaderValue", Direction = SoapHeaderDirection.Out)]
        [SoapDocumentMethodAttribute(
            "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices/ListChildren", 
            RequestNamespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices", 
            ResponseNamespace = "http://schemas.microsoft.com/sqlserver/2005/06/30/reporting/reportingservices", 
            Use = System.Web.Services.Description.SoapBindingUse.Literal, 
            ParameterStyle = SoapParameterStyle.Wrapped)]
        [return: XmlArrayAttribute("CatalogItems")]
        public CatalogItem[] ListChildren(string Item, bool Recursive)
        {
            var results = Invoke("ListChildren", new object[] {Item, Recursive});
            return (CatalogItem[])results[0];
        }
    }
}