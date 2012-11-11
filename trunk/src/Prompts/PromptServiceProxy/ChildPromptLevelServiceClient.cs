using System;
using System.Collections.ObjectModel;
using System.Windows;
using Prompts.Prompting.Model;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;
using ServiceStack.ServiceClient.Web;

namespace Prompts.PromptServiceProxy
{
    public class ChildPromptLevelServiceClient :IChildPromptLevelServiceClient
    {
        private readonly JsonRestClientAsync _client;
        private readonly string _uri;

        public ChildPromptLevelServiceClient(JsonRestClientAsync client, string uri)
        {
            _uri = uri;
            _client = client;
        }

        public void GetChildren2Async(
            string promptName, 
            string parameterName, 
            ParameterValue parameterValue, 
            Action<PromptLevel> callback,
            Action<string> errorCallback)
        {
            GetChildrenAsync(
                promptName,
                parameterName,
                new ObservableCollection<ParameterValue> {parameterValue},
                callback,
                errorCallback);
        }

        public void GetChildrenAsync(
            string promptName, 
            string parameterName, 
            ObservableCollection<ParameterValue> parameterValues, 
            Action<PromptLevel> callback,
            Action<string> errorCallback)
        {
            _client.PostAsync<PromptLevel>(
                _uri,
                new ChildPromptItemsRequest
                    {
                        PromptName = promptName,
                        ParameterName = parameterName,
                        ParameterValues = parameterValues
                    },
                result => Deployment.Current.Dispatcher.BeginInvoke(() => callback(result)),
                (result, e) => Deployment.Current.Dispatcher.BeginInvoke(() => errorCallback(e.Message)));
        }

        public void GetChildrenForRecursive(
            string promptName, 
            string parameterName, 
            ParameterValue parameterValue, 
            Action<PromptLevel> callback,
            Action<string> errorCallback)
        {
            _client.PostAsync<PromptLevel>(
                _uri,
                    new RecursiveChildPromptItemsRequest
                        {
                        PromptName = promptName,
                        ParameterName = parameterName,
                        ParameterValue = parameterValue
                    },
                result => Deployment.Current.Dispatcher.BeginInvoke(() => callback(result)),
                (result, e) => Deployment.Current.Dispatcher.BeginInvoke(() => errorCallback(e.Message)));
        }
    }
}
