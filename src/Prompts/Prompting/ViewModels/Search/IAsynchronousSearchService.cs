using System;
using System.Collections.ObjectModel;

namespace Prompts.Prompting.ViewModels.Search
{
    public interface IAsynchronousSearchService
    {
        void Search(
            string searchExpression,
            Action<ObservableCollection<ISearchablePromptItem>> searchResultCallback,
            Action<string> errorCallback);
    }
}
