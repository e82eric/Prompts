using System;
using System.Collections.ObjectModel;
using Prompts.Service.PromptService;

namespace Prompts.PromptServiceProxy
{
    public interface IPromptSelectionServiceClient
    {
        void SetPromptSelectionsAsync(
            string path, 
            ObservableCollection<PromptSelectionInfo> promptSelections,
            Action<string> callback,
            Action<string> errorCallback);
    }
}