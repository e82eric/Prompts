using System;
using System.Collections.ObjectModel;

namespace Prompts.Prompting.ViewModels.Search.Implementation
{
    public class AsynchronousSearch : IAsynchronousSearch
    {
        private readonly string _promptName;
        private readonly string _parameterName;
        private readonly IChildPromptItemsService _childPromptItemsService;
        private readonly ISearch _search;

        public AsynchronousSearch(
            string promptName
            , string parameterName
            , IChildPromptItemsService childPromptItemsService
            , ISearch search)
        {
            _search = search;
            _childPromptItemsService = childPromptItemsService;
            _parameterName = parameterName;
            _promptName = promptName;
        }

        public void Execute(
            Action<ObservableCollection<ISearchablePromptItem>> searchResultCallback,
            Action<string> errorCallback)
        {
            _childPromptItemsService.GetChildren(
                _promptName
                , _parameterName
                , _search.SearchString
                , c => searchResultCallback(_search.Execute(c))
                , errorCallback);
        }
    }
}