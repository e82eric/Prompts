using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Prompts.Prompting.Construction.Implementation
{
    public class DropDownProvider : ISingleSelectPromptProvider<ISearchablePromptItem>
    {
        public IPrompt Get(string name, string label, ObservableCollection<ISearchablePromptItem> availableItems, IPromptItem defaultSelection)
        {
            var searchablePromptItemCollection = new SearchablePromptItems(availableItems);
            var searchProvider = new SearchProvider();
            var searchStringParser = new SearchStringParser<ISearch>("*", searchProvider);
            var searchService = new SearchService(searchablePromptItemCollection, searchStringParser);

            return new SearchableSingleSelectPrompt(name, label, defaultSelection, searchService);
        }
    }
}