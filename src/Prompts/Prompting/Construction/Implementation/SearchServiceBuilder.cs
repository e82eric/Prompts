using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Prompts.Prompting.Construction.Implementation
{
    public class SearchServiceBuilder : ISearchServiceBuilder
    {
        private readonly ISearchStringParser<ISearch> _searchStringParser;

        public SearchServiceBuilder(ISearchStringParser<ISearch> searchStringParser)
        {
            _searchStringParser = searchStringParser;
        }

        public ISearchService Build(ISearchablePromptItemCollection searchablePromptItemCollection)
        {
            return new SearchService(searchablePromptItemCollection, _searchStringParser);
        }
    }
}