using System.Windows;
using System.Windows.Controls;

namespace Prompts.Prompting.Controls
{
    public class TreeComboBox : ComboBox
    {
        public object SelectedTreeItem
        {
            get { return GetValue(SelectedTreeItemProperty); } 
            set
            {
                SetValue(SelectedTreeItemProperty, value);
                SetSelectedItemDisplayText();
            }
        }

        public DependencyProperty SelectedTreeItemProperty
            = DependencyProperty.Register(
                "SelectedTreeItem"
                , typeof (object)
                , typeof (TreeComboBox)
                , new PropertyMetadata(null, OnSelectedItemChanged2));

        private static void OnSelectedItemChanged2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TreeComboBox)d).SelectedTreeItem = e.NewValue;
        }

        private PromptTreeControl _treeview;

        internal void SetSelectedItemText()
        {
            if (_treeview.SelectedItem != null)
            {
                if (SelectedTreeItem != _treeview.SelectedItem)
                {
                    SelectedTreeItem = _treeview.SelectedItem;
                }

                SetSelectedItemDisplayText();
            }
            else if(SelectedTreeItem != null)
            {
                SetSelectedItemDisplayText();
            }
        }

        private void SetSelectedItemDisplayText()
        {
            var selectedItemDisplay = (ContentPresenter)GetTemplateChild("ContentPresenter2");

            if(selectedItemDisplay != null)
            {
                selectedItemDisplay.Content = new ContentControl
                {
                    DataContext = SelectedTreeItem
                    ,
                    Content = ItemTemplate.LoadContent()
                };
            }
        }

        public TreeComboBox()
        {
            DefaultStyleKey = typeof (TreeComboBox);
        }

        public override void OnApplyTemplate()
        {
            _treeview = (PromptTreeControl)GetTemplateChild("InnerTreeView");
            _treeview.SelectedValuePath = "SelectedItem";
            _treeview.SelectedItemChanged += (s, e) =>
                {
                    SetSelectedItemText();
                    IsDropDownOpen = false;
                };

            SetSelectedItemText();

            base.OnApplyTemplate();
        }
    }
}
