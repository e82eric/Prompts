using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;

namespace Prompts.Prompting.Construction
{
    public interface ISingleSelectPromptProvider<T>
    {
        IPrompt Get(string name, string label, ObservableCollection<T> availableItems, IPromptItem defaultSelection);
    }
}