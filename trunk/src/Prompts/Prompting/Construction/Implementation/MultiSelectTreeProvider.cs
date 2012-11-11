using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class MultiSelectTreeProvider : IShoppingCartProvider<ITreeNode>
    {
        public IPrompt Get(
            string label, 
            string name, 
            ObservableCollection<ITreeNode> availableItems, 
            ObservableCollection<ITreeNode> selectedItems)
        {
            return new MultiSelectHierarchy(name, label, availableItems, selectedItems);
        }

        public IPrompt Get(PromptInfo promptInfo, ObservableCollection<ITreeNode> availableItems, ObservableCollection<ITreeNode> defaultSelections)
        {
            return new MultiSelectHierarchy(
                promptInfo.Name, 
                promptInfo.Label, 
                availableItems, 
                defaultSelections);
        }
    }
}
