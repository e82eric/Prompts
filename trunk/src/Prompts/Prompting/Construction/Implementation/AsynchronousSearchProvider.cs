using System;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Prompts.Prompting.Construction.Implementation
{
    public class AsynchronousSearchProvider : ISearchProvider<IAsynchronousSearch>
    {
        private readonly string _promptName;
        private readonly string _parameterName;
        private readonly IChildPromptItemsService _childPromptItemsService;
        private readonly ISearchProvider<ISearch> _searchProvider;

        public AsynchronousSearchProvider(
            string promptName, 
            string parameterName,
            IChildPromptItemsService childPromptItemsService,
            ISearchProvider<ISearch> searchProvider)
        {
            _searchProvider = searchProvider;
            _childPromptItemsService = childPromptItemsService;
            _parameterName = parameterName;
            _promptName = promptName;
        }

        public IAsynchronousSearch CreateContainsSearch(string searchString)
        {
            return CreateSearch(() => _searchProvider.CreateContainsSearch(searchString));
        }

        public IAsynchronousSearch CreateEndsWithSearch(string searchString)
        {
            return CreateSearch(() => _searchProvider.CreateEndsWithSearch(searchString));
        }

        public IAsynchronousSearch CreateEqualsSearch(string searchString)
        {
            return CreateSearch(() => _searchProvider.CreateEqualsSearch(searchString));
        }

        public IAsynchronousSearch CreateNullSearch()
        {
            return CreateSearch(() => _searchProvider.CreateNullSearch());
        }

        public IAsynchronousSearch CreateStartsWithSearch(string searchString)
        {
            return CreateSearch(() => _searchProvider.CreateStartsWithSearch(searchString));
        }

        private AsynchronousSearch CreateSearch(Func<ISearch> createBaseSearchDelegate)
        {
            return new AsynchronousSearch(_promptName, _parameterName, _childPromptItemsService, createBaseSearchDelegate());
        }
    }
}