using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace Prompts.Prompting.Controls
{
    public class PromptTreeViewItem : TreeViewItem, ITreeItem
    {
        private bool _isSelected2;
        private List<ITreeItem> _treeItems;

        public PromptTreeViewItem()
        {
            DefaultStyleKey = typeof (PromptTreeViewItem);
        }

        public static DependencyProperty IsSelected2Property =
            DependencyProperty.Register(
                "IsSelected2",
                typeof (bool),
                typeof (PromptTreeViewItem),
                new PropertyMetadata(false, OnIsSelected));

        private static void OnIsSelected(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PromptTreeViewItem)d).IsSelected2 = (bool)e.NewValue;
        }

        public bool IsSelected2
        {
            get { return _isSelected2; }
            set
            {
                _isSelected2 = value;
                SetValue(IsSelected2Property, value);
                ApplyIsSelectedFormating();
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            _treeItems = new List<ITreeItem>();
            var tvi = new PromptTreeViewItem();

            var expandedBinding = new Binding("IsExpanded") { Mode = BindingMode.TwoWay };
            tvi.SetBinding(IsExpandedProperty, expandedBinding);

            var isEnabledBinding = new Binding("IsEnabled") { Mode = BindingMode.OneWay };
            tvi.SetBinding(IsEnabledProperty, isEnabledBinding);

            var isSelectedBinding = new Binding("IsSelected") { Mode = BindingMode.TwoWay };
            tvi.SetBinding(IsSelected2Property, isSelectedBinding);

            tvi.ParentTreeView = ParentTreeView;

            _treeItems.Add(tvi);
            return tvi;
        }

        private void ApplyIsSelectedFormating()
        {
            var selectionRectangle = (Rectangle)GetTemplateChild("Selection");
                
            if(selectionRectangle != null)
            {
                selectionRectangle.Opacity = _isSelected2 ? .7 : 0;  
            }
        }

        public override void OnApplyTemplate()
        {
            
            base.OnApplyTemplate();
            ApplyIsSelectedFormating();
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!e.Handled)
            {
                ParentTreeView.SelectItem(this);
                e.Handled = true;
            }
        }

        public PromptTreeControl ParentTreeView { get; set; }

        public ICollection<ITreeItem> Children
        {
            get
            {
                if (Items == null)
                {
                    return null;
                }

                var children = new List<ITreeItem>();
                foreach (var item in Items)
                {
                    if(ItemContainerGenerator == null)
                    {
                        throw new NullReferenceException();
                    }
                    var container = (ITreeItem) ItemContainerGenerator.ContainerFromItem(item);
                    if (container != null)
                    {
                        children.Add(container);
                    }
                }
                return children;
            }
        }
    }
}