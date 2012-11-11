using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Prompts.Prompting.Controls
{
    public class PromptTreeControl : TreeView
    {
        private readonly MultiSelector _multiSelector;
        private readonly List<ITreeItem> _treeItems;
        private ITreeItem _oldSelectedItem;

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable) GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public static DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(
                "SelectedItems",
                typeof (IEnumerable<object>),
                typeof (PromptTreeControl),
                null);

        public PromptTreeControl()
        {
            _treeItems = new List<ITreeItem>();

            _multiSelector = new MultiSelector(
                new RangeSelector(new TreeItemHierarchyFlattener())
                , new InverseSelector(new TreeItemHierarchyFlattener())
                , new Selector(new TreeItemHierarchyFlattener2()));
        }

        public void SelectItem(ITreeItem item)
        {
            if (_oldSelectedItem == null)
            {
                _oldSelectedItem = item;
            }
            _multiSelector.Select(Keyboard.Modifiers, _treeItems, item, _oldSelectedItem);
            _oldSelectedItem = item;
            SelectedItems = GetSelectedItems();
        }

        private IEnumerable<object> GetSelectedItems()
        {
            var selectedItems = new List<object>();
            foreach (var treeItem in _treeItems)
            {
                if(treeItem.IsSelected2)
                {
                    var dataContext = ((TreeViewItem) treeItem).DataContext;
                    selectedItems.Add(dataContext);
                }
                AddSelectedItemsFromChildren(treeItem, selectedItems);
            }
            return selectedItems;
        }

        private static void AddSelectedItemsFromChildren(ITreeItem treeItem, ICollection<object> selectedItems)
        {
            if(treeItem.Children != null)
            {
                foreach (var child in treeItem.Children)
                {
                    if (child.IsSelected2)
                    {
                        var dataContext = ((TreeViewItem) treeItem).DataContext;
                        selectedItems.Add(dataContext);
                    }
                    AddSelectedItemsFromChildren(child, selectedItems);
                }
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var tvi = new PromptTreeViewItem();
            var expandedBinding = new Binding("IsExpanded") { Mode = BindingMode.TwoWay };
            tvi.SetBinding(TreeViewItem.IsExpandedProperty, expandedBinding);

            var isEnabledBinding = new Binding("IsEnabled") { Mode = BindingMode.OneWay };
            tvi.SetBinding(IsEnabledProperty, isEnabledBinding);

            var isSelectedBinding = new Binding("IsSelected") { Mode = BindingMode.TwoWay };
            tvi.SetBinding(PromptTreeViewItem.IsSelected2Property, isSelectedBinding);

            tvi.ParentTreeView = this;

            _treeItems.Add(tvi);
            return tvi;
        }
    }
}
