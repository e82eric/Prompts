using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;

namespace Prompts.Prompting.Construction.Implementation
{
    public class SingleSelectTreeProvider : ISingleSelectPromptProvider<ITreeNode>
    {
        public IPrompt Get(string name, string label, ObservableCollection<ITreeNode> availableItems, IPromptItem defaultSelection)
        {
            return new SingleSelectHierarchy(name, label, availableItems, defaultSelection);
        }
    }
}
