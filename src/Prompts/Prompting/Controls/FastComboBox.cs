using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Prompts.Prompting.Controls
{
    public class FastComboBox : ComboBox
    {

        public IEnumerable ItemsSource2
        {
            get { return (IEnumerable)GetValue(ItemsSource2Property); }
            set
            {
                SetValue(ItemsSource2Property, value);
                SetListBoxItems();
            }
        }

        public DependencyProperty ItemsSource2Property;

        private static void OnItemsSource2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FastComboBox) d).ItemsSource2 = (IEnumerable)e.NewValue;
        }

        public object SelectedItem2
        {
            get { return GetValue(SelectedItem2Property); }
            set
            {
                SetValue(SelectedItem2Property, value);
                SetSelectedItemDisplayText();
            }
        }

        public DependencyProperty SelectedItem2Property
            = DependencyProperty.Register(
                "SelectedItem2"
                , typeof(object)
                , typeof(FastComboBox)
                , new PropertyMetadata(null, OnSelectedItemChanged2));

        private static void OnSelectedItemChanged2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FastComboBox)d).SelectedItem2 = e.NewValue;
        }

        private ListBox _listBox;

        public FastComboBox()
        {
            var propertyMetadata = new PropertyMetadata(OnItemsSource2Changed);

            ItemsSource2Property = DependencyProperty.Register(
                "ItemsSource2"
                , typeof(IEnumerable)
                , typeof(FastComboBox)
                , propertyMetadata);

            DefaultStyleKey = typeof(FastComboBox);
        }

        internal void SetSelectedItemText()
        {
            if (_listBox.SelectedItem != null)
            {
                if (SelectedItem2 != _listBox.SelectedItem)
                {
                    SelectedItem2 = _listBox.SelectedItem;
                }

                SetSelectedItemDisplayText();
            }
            else if (SelectedItem2 != null)
            {
                SetSelectedItemDisplayText();
            }
        }

        private void SetSelectedItemDisplayText()
        {
            var selectedItemDisplay = (ContentPresenter)GetTemplateChild("ContentPresenter2");

            if (selectedItemDisplay != null)
            {
                selectedItemDisplay.Content = new ContentControl
                    {
                        DataContext = SelectedItem2,
                        Content = ItemTemplate.LoadContent()
                    };
            }
        }

        private void SetListBoxItems()
        {
            if (_listBox != null)
            {
                _listBox.ItemsSource = ItemsSource2;
                if (ItemsSource2 != null)
                {
                    _listBox.UpdateLayout();
                    _listBox.ScrollIntoView(_listBox.Items.FirstOrDefault());
                }
            }
        }



        public override void OnApplyTemplate()
        {
            _listBox = (ListBox)GetTemplateChild("InnerListBox");
            _listBox.SelectedValuePath = "SelectedItem";
            _listBox.ItemsSource = ItemsSource2;
            _listBox.SelectionChanged += (s, e) =>
                {
                    SetSelectedItemText();
                    IsDropDownOpen = false;
                };

            SetSelectedItemText();

            base.OnApplyTemplate();
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            _listBox.Width = arrangeBounds.Width;

            return base.ArrangeOverride(arrangeBounds);
        }
    }
}