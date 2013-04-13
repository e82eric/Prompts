using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prompts.Service.PromptService;
using ServiceStack.ServiceClient.Web;

namespace Prompts.PromptServiceProxy
{
    public class PromptSelectionServiceClient : IPromptSelectionServiceClient
    {
        private readonly JsonRestClientAsync _client;
        private readonly string _uri;

        public PromptSelectionServiceClient(JsonRestClientAsync client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        public void SetPromptSelectionsAsync(
            string path, 
            ObservableCollection<PromptSelectionInfo> promptSelections, 
            Action<string> callback,
            Action<string> errorCallback)
        {
            _client.PostAsync<string>(
                _uri,
                new SetPromptSelectionsRequest
                    {
                        Path = path,
                        PromptSelections = promptSelections
                    },
                result => Deployment.Current.Dispatcher.BeginInvoke(() => callback(result)),
                (result, e) => Deployment.Current.Dispatcher.BeginInvoke(() => errorCallback(e.Message)));
        }
    }
}