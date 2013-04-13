using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Prompts.Prompting.ViewModels;

namespace Prompts.Prompting.Controls
{
    public class MultiSelectListbox : ListBox
    {
        public MultiSelectListbox()
        {
            SelectionChanged += (s, e) =>
                {
                    if (SelectedItems != null)
                    {
                        if (SelectedItems.Count > 0)
                        {
                            if (SelectedItems[0] is ISearchablePromptItem)
                            {
                                Selections = new ObservableCollection<ISearchablePromptItem>(
                                    SelectedItems.Cast<ISearchablePromptItem>());
                            }

                            if (SelectedItems[0] is ITreeNode)
                            {
                                Selections = new ObservableCollection<ITreeNode>(
                                    SelectedItems.Cast<ITreeNode>());
                            }
                        }
                    }
                };
        }

        public IList Selections
        {
            get { return (IList) GetValue(SelectionsProperty); }
            set { SetValue(SelectionsProperty, value); }
        }

        public static DependencyProperty SelectionsProperty = DependencyProperty.Register(
            "Selections",
            typeof (IList),
            typeof (MultiSelectListbox),
            null);
    }
}