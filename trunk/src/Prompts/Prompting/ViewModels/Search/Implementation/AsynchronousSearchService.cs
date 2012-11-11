using System;
using System.Collections.ObjectModel;

namespace Prompts.Prompting.ViewModels.Search.Implementation
{
    public class AsynchronousSearchService : IAsynchronousSearchService
    {
        private readonly ISearchStringParser<IAsynchronousSearch> _searchStringParser;

        public AsynchronousSearchService(ISearchStringParser<IAsynchronousSearch> searchStringParser)
        {
            _searchStringParser = searchStringParser;
        }

        public void Search(
            string searchExpression,
            Action<ObservableCollection<ISearchablePromptItem>> searchResultCallback,
            Action<string> errorCallback)
        {
            var search = _searchStringParser.Parse(searchExpression);
            search.Execute(searchResultCallback, errorCallback);
        }
    }
}