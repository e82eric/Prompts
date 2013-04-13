using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prompts.Prompting.ViewModels.Search;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class SearchablePromptItems : ISearchablePromptItemCollection
    {
        private readonly ObservableCollection<ISearchablePromptItem> _enumeration;

        public SearchablePromptItems(ObservableCollection<ISearchablePromptItem> enumeration)
        {
            _enumeration = enumeration;
        }

        public ObservableCollection<ISearchablePromptItem> Enumeration
        {
            get { return _enumeration; }
        }

        public ObservableCollection<ISearchablePromptItem> LabelStartsWith(string searchString)
        {
            return CreateObservableCollectionWhere(e => e.LabelStartsWithCi(searchString));
        }

        public ObservableCollection<ISearchablePromptItem> LabelEndsWith(string searchString)
        {
            return CreateObservableCollectionWhere(e => e.LabelEndsWithCi(searchString));
        }

        public ObservableCollection<ISearchablePromptItem> LabelContains(string searchString)
        {
            return CreateObservableCollectionWhere(e => e.LabelContainsCi(searchString));
        }

        public ObservableCollection<ISearchablePromptItem> LabelEquals(string searchString)
        {
            return CreateObservableCollectionWhere(e => e.LabelEqualsCi(searchString));
        }

        private ObservableCollection<ISearchablePromptItem> CreateObservableCollectionWhere(Func<ISearchablePromptItem, bool> predicate)
        {
            return new ObservableCollection<ISearchablePromptItem>(_enumeration.Where(predicate));
        }
    }
}