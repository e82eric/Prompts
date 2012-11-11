using System;
using System.Collections.Generic;
using System.Windows;
using Prompts.Service.PromptService;
using ServiceStack.ServiceClient.Web;

namespace Prompts.PromptServiceProxy
{
    public class PromptServiceClient :  IPromptServiceClient
    {
        private readonly JsonRestClientAsync _client;
        private readonly string _uri;

        public PromptServiceClient(JsonRestClientAsync client, string uri) 
        {
            _uri = uri;
            _client = client;
        }

        public void GetPromptsForReportAsync(
            string path, 
            Action<IEnumerable<PromptInfo>> callback, 
            Action<string> errorCallback)
        {
            _client.PostAsync<IEnumerable<PromptInfo>>(
                _uri,
                new PromptsRequest { Path = path },
                result => Deployment.Current.Dispatcher.BeginInvoke(() => callback(result)),
                (result, e) => Deployment.Current.Dispatcher.BeginInvoke(() => errorCallback(e.Message)));
        }
    }
}
