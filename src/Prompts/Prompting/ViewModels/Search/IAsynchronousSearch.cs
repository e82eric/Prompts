using System;
using System.Collections.ObjectModel;

namespace Prompts.Prompting.ViewModels.Search
{
    public interface IAsynchronousSearch
    {
        void Execute(
            Action<ObservableCollection<ISearchablePromptItem>> searchResultCallback,
            Action<string> errorCallback);
    }
}