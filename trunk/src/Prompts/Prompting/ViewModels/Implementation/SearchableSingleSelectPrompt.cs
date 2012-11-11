using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prompts.Infastructure;
using Prompts.Prompting.ViewModels.Search;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class SearchableSingleSelectPrompt : SingleSelectPrompt<ISearchablePromptItem>
    {
        private readonly ISearchService _searchService;

        public SearchableSingleSelectPrompt(
            string name, 
            string label, 
            IPromptItem defaultSelection,
            ISearchService searchService) 
            : base(name, label, searchService.GetAll(), defaultSelection)
        {
            _searchService = searchService;
            Search = new RelayCommand(OnSearch);
            AvailableItems = new ObservableCollection<ISearchablePromptItem>(searchService.GetAll().ToArray());
        }

        public string SearchString { get; set; }

        public ICommand Search { get; private set; }

        private void OnSearch()
        {
            AvailableItems = _searchService.Search(SearchString);
        }
    }
}