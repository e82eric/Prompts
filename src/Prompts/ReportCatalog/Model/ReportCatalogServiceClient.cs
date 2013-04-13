using System;
using System.Collections.Generic;
using System.Windows;
using ServiceStack.ServiceClient.Web;

namespace Prompts.ReportCatalog.Model
{
    public class ReportCatalogServiceClient : IReportCatalogServiceClient
    {
        private readonly JsonRestClientAsync _client;
        private readonly string _uri;

        public ReportCatalogServiceClient(JsonRestClientAsync client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        public void GetReportCatalogInfoAsync(Action<IEnumerable<CatalogItemInfo>> callBack, Action<string> errorCallback)
        {
            _client.GetAsync<IEnumerable<CatalogItemInfo>>(
                _uri,
                result => Deployment.Current.Dispatcher.BeginInvoke(() => callBack(result)),
                (result, e) => Deployment.Current.Dispatcher.BeginInvoke(() => errorCallback(e.Message)));
        }
    }
}
