using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;

namespace Prompts.Prompting.Construction
{
    public interface ICasscadingSearchProvider
    {
        IMultiSelectPrompt Get(
            string label, 
            string promptName, 
            string parameterName, 
            ObservableCollection<ISearchablePromptItem> availableItems, 
            ObservableCollection<ISearchablePromptItem> defaultSelections);
    }
}