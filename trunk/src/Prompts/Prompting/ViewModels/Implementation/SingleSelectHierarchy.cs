using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class SingleSelectHierarchy : SingleSelectPrompt<ITreeNode>
    {
        public SingleSelectHierarchy(
            string name
            , string label
            , ObservableCollection<ITreeNode> promptItems
            , IPromptItem defaultSelection)
            : base(
                name
                , label
                , promptItems
                , defaultSelection)
        {
        }

        protected override IPromptItem FindSelectedItemInEnumeration(IPromptItem itemToSelect)
        {
            var itemsToSelect = new List<IPromptItem>();
            RecurseTree(AvailableItems, n =>
                {
                    if (itemToSelect != null)
                    {
                        if(n == itemToSelect)
                        {
                            n.IsSelected = true;
                            itemsToSelect.Add(n);
                        }
                        else
                        {
                            n.IsSelected = false;
                        }
                    }
                });

            return itemsToSelect.SingleOrDefault();
        }

        private static void RecurseTree(IEnumerable<ITreeNode> nodes, Action<ITreeNode> action)
        {
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    action(node);
                    RecurseTree(node.Children, action);
                }
            }
        }
    }
}