using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public interface IShoppingCartProvider<T>
    {
        IPrompt Get(
            PromptInfo promptInfo,
            ObservableCollection<T> availableItems,
            ObservableCollection<T> defaultSelections);
    }
}