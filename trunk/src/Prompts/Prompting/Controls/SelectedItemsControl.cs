using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Prompts.Prompting.Controls
{
    public class SelectedItemsControl : ContentControl
    {
        public SelectedItemsControl()
        {
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set 
            { 
                SetValue(ItemsSourceProperty, value); 
            }
        }

        public static DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable),
                typeof (SelectedItemsControl),
                new PropertyMetadata(null, OnItemsChanged));

        public DataTemplate ItemsTemplate
        {
            get { return (DataTemplate) GetValue(ItemsTemplateProperty); }
            set { SetValue(ItemsTemplateProperty, value); }
        }

        public static DependencyProperty ItemsTemplateProperty =
            DependencyProperty.Register(
                "ItemsTemplate",
                typeof (DataTemplate),
                typeof (SelectedItemsControl),
                new PropertyMetadata(null, OnItemsChanged));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set 
            { 
                SetValue(SelectedItemProperty, value); 
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof (object),
                typeof (SelectedItemsControl),
                new PropertyMetadata(OnSelectedItem));

        private Dictionary<object, ContentControl> _controls;

        private static void OnSelectedItem(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SelectedItemsControl) d;
            control.SelectItem();
        }

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SelectedItemsControl) d;
            control.Init();
        }

        internal void Init()
        {
            if (ItemsSource != null && ItemsTemplate != null && SelectedItem != null)
            {
                _controls = new Dictionary<object, ContentControl>();

                foreach (var item in ItemsSource)
                {
                    var contentControl2 = new WeakReference(
                        new ContentControl
                            {
                                Content = ItemsTemplate.LoadContent(),
                                DataContext = item,
                                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                                VerticalContentAlignment = VerticalAlignment.Stretch,
                            });

                    _controls.Add(item, (ContentControl)contentControl2.Target);
                }
            }
        }

        internal void SelectItem()
        {
            if (ItemsSource != null && ItemsTemplate != null && SelectedItem != null)
            {
                if(_controls == null)
                {
                    Init();
                }
                ContentControl control;

                if(_controls == null)
                {
                    throw new NullReferenceException();
                }
                _controls.TryGetValue(SelectedItem, out control);

               Content = control;
            }
        }
    }
}