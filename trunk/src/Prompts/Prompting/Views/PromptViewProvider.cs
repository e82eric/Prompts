using System.Windows.Controls;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;

namespace Prompts.Prompting.Views
{
    public class PromptViewProvider : IPromptViewProvider
    {
        private readonly IPromptViewProvider _multiSelectHierarchyViewProvider;
        private readonly IPromptViewProvider _multiSelectViewProvider;
        private readonly IPromptViewProvider _casscadingSearchViewProvider;
        private readonly IPromptViewProvider _singleSelectViewProvider;
        private readonly IPromptViewProvider _singleSelectHierarchyViewProvider;
        private readonly IPromptViewProvider _emptyViewProvider;

        public PromptViewProvider(
            IPromptViewProvider multiSelectHierarchyViewProvider,
            IPromptViewProvider multiSelectViewProvider,
            IPromptViewProvider casscadingSearchViewProvider,
            IPromptViewProvider singleSelectViewProvider,
            IPromptViewProvider singleSelectHierarchyViewProvider,
            IPromptViewProvider emptyViewProvider)
        {
            _emptyViewProvider = emptyViewProvider;
            _singleSelectHierarchyViewProvider = singleSelectHierarchyViewProvider;
            _singleSelectViewProvider = singleSelectViewProvider;
            _casscadingSearchViewProvider = casscadingSearchViewProvider;
            _multiSelectViewProvider = multiSelectViewProvider;
            _multiSelectHierarchyViewProvider = multiSelectHierarchyViewProvider;
        }

        public UserControl Get(object viewModel)
        {
            if(viewModel is MultiSelectHierarchy)
            {
                return _multiSelectHierarchyViewProvider.Get(viewModel);
            }
            if(viewModel is MultiSelectSearch)
            {
                return _multiSelectViewProvider.Get(viewModel);
            }
            if(viewModel is MultiSelectCasscadingSearch)
            {
                return _casscadingSearchViewProvider.Get(viewModel);
            }
            if (viewModel is SingleSelectPrompt<ISearchablePromptItem>)
            {
                return _singleSelectViewProvider.Get(viewModel);
            }
            if(viewModel is EmptyPrompt)
            {
                return _emptyViewProvider.Get(viewModel);
            }
            if(viewModel is SingleSelectHierarchy)
            {
                return _singleSelectHierarchyViewProvider.Get(viewModel);
            }
            return null;
        }
    }
}
