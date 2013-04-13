using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class CasscadingSearchProvider : IShoppingCartProvider<ISearchablePromptItem>
    {
        private readonly IAsynchronousSearchServiceBuilder _asynchronousSearchServiceBuilder;

        public CasscadingSearchProvider(IAsynchronousSearchServiceBuilder asynchronousSearchServiceBuilder)
        {
            _asynchronousSearchServiceBuilder = asynchronousSearchServiceBuilder;
        }

        public IPrompt Get(
            PromptInfo promptInfo,
            ObservableCollection<ISearchablePromptItem> availableItems,
            ObservableCollection<ISearchablePromptItem> defaultSelections)
        {
            var promptName = promptInfo.Name;
            var parameterName = promptInfo.PromptLevelInfo.ParameterName;
            var label = promptInfo.Label;

            var searchService = _asynchronousSearchServiceBuilder.Build(promptName, parameterName);

            return new MultiSelectCasscadingSearch(
                label
                , promptName
                , searchService
                , availableItems
                , defaultSelections);
        }
    }
}