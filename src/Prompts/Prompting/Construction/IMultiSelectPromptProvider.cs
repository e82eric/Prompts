using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;

namespace Prompts.Prompting.Construction
{
    public interface IMultiSelectPromptProvider<T> where T : IPromptItem
    {
        IPrompt Get(
            string label, 
            string name, 
            ObservableCollection<T> availableItems, 
            ObservableCollection<T> selectedItems);
    }
}