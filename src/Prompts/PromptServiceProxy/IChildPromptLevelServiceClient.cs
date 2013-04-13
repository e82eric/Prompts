using System;
using System.Collections.ObjectModel;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.Model
{
    public interface IChildPromptLevelServiceClient
    {
        void GetChildren2Async(
            string promptName, 
            string parameterName, 
            ParameterValue parameterValue, 
            Action<PromptLevel> callback,
            Action<string> errorCallback);

        void GetChildrenAsync(
            string promptName,
            string parameterName,
            ObservableCollection<ParameterValue> parameterValues,
            Action<PromptLevel> callback,
            Action<string> errorCallback);

        void GetChildrenForRecursive(
            string promptName, 
            string parameterName, 
            ParameterValue parameterValue, 
            Action<PromptLevel> callback,
            Action<string> errorCallback);
    }
}