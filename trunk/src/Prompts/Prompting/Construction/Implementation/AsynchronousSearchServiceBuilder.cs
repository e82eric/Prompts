using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Prompts.Prompting.Construction.Implementation
{
    public class AsynchronousSearchServiceBuilder : IAsynchronousSearchServiceBuilder
    {
        private readonly IChildPromptItemsService _childPromptItemsService;
        private readonly ISearchProvider<ISearch> _searchProvider;
        private readonly string _specialCharecter;

        public AsynchronousSearchServiceBuilder(string specialCharecter
            , IChildPromptItemsService childPromptItemsService
            , ISearchProvider<ISearch> searchProvider)
        {
            _specialCharecter = specialCharecter;
            _searchProvider = searchProvider;
            _childPromptItemsService = childPromptItemsService;
        }

        public IAsynchronousSearchService Build(string promptName, string parameterName)
        {
            return new AsynchronousSearchService
                (new SearchStringParser<IAsynchronousSearch>
                     (_specialCharecter
                      , new AsynchronousSearchProvider
                            (promptName
                             , parameterName
                             , _childPromptItemsService
                             , _searchProvider)));
        }
    }
}