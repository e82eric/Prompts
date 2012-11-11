using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class SearchableShoppingCartProvider : IShoppingCartProvider<ISearchablePromptItem>
    {
        public IPrompt Get(
            PromptInfo promptInfo, 
            ObservableCollection<ISearchablePromptItem> availableItems, 
            ObservableCollection<ISearchablePromptItem> defaultSelections)
        {
            var searchablePromptItemCollection = new SearchablePromptItems(availableItems);
            var searchProvider = new SearchProvider();
            var searchStringParser = new SearchStringParser<ISearch>("*", searchProvider);
            var searchService = new SearchService(searchablePromptItemCollection, searchStringParser);
            
            return new MultiSelectSearch(
                promptInfo.Label, 
                promptInfo.Name, 
                searchService, 
                defaultSelections);
        }
    }
}
