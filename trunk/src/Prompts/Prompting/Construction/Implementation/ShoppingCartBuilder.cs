using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class ShoppingCartBuilder<T> : IPromptBuilder where T : IPromptItem
    {
        private readonly IPromptItemProvider<T> _promptItemProvider;
        private readonly IShoppingCartProvider<T> _shoppingCartProvider;

        public ShoppingCartBuilder(
            IPromptItemProvider<T> promptItemProvider,
            IShoppingCartProvider<T> shoppingCartProvider)
        {
            _shoppingCartProvider = shoppingCartProvider;
            _promptItemProvider = promptItemProvider;
        }

        public IPrompt BuildFrom(PromptInfo promptInfo)
        {
            var items = new ObservableCollection<T>();
            var defaultItems = new ObservableCollection<T>();

            foreach (var availableItem in promptInfo.PromptLevelInfo.AvailableItems)
            {
                var promptItem = _promptItemProvider.Get(
                    promptInfo.Name
                    , promptInfo.PromptLevelInfo.ParameterName
                    , availableItem);

                foreach (var defaultValidValue in promptInfo.DefaultValues)
                {
                    if (availableItem.Value == defaultValidValue.Value)
                    {
                        if (defaultValidValue.IsAllMember)
                        {
                            promptItem.IsDefaultAll = true;
                        }
                        defaultItems.Add(promptItem);
                    }
                }

                items.Add(promptItem);
            }

            return _shoppingCartProvider.Get(promptInfo, items, defaultItems);
        }
    }
}