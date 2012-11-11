using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Linq;

namespace Prompts.Prompting.Controls
{
    public class UniformItemsControl : Control, IUniformItemsControl
    {
        private Grid _layoutRoot;
        private readonly Dictionary<object, AnnimationHelper> _annimationItems;
        private readonly UniformItemsController _controller;

        public UniformItemsControl()
        {
            DefaultStyleKey = typeof(UniformItemsControl);
            _controller = new UniformItemsController(this);
            _annimationItems = new Dictionary<object, AnnimationHelper>();
        }

        public void CreateItems(IEnumerable<object> items)
        {
            ApplyTemplate();
            _annimationItems.Clear();
            _layoutRoot.Children.Clear();
            _layoutRoot.RowDefinitions.Clear();
            Resources.Clear();
            var rowNumber = 0;

            foreach (var item in items)
            {
                var rowDefinition = new RowDefinition();
                _layoutRoot.RowDefinitions.Add(rowDefinition);

                var uniformItem = CreateUniformItem(item, rowNumber);
                uniformItem.Name = string.Format("Item{0}", rowNumber);
                _layoutRoot.Children.Add(uniformItem);

                var scaleTransform = new ScaleTransform
                    {
                        ScaleX = 1
                        , ScaleY = 1
                    };

                uniformItem.RenderTransform = scaleTransform;
                uniformItem.RenderTransformOrigin = new Point(.5, .5);

                uniformItem.SetValue(Grid.RowProperty, rowNumber);

                var growStoryboard = new Storyboard();
                growStoryboard.Completed += GrowStoryboardCompleted;

                var fadeInAnnimation = new DoubleAnimation
                    {
                        From = 0, 
                        To = 1, 
                        Duration = new Duration(TimeSpan.FromMilliseconds(200))
                    };
                Storyboard.SetTargetName(fadeInAnnimation, uniformItem.Name);
                Storyboard.SetTargetProperty(fadeInAnnimation, new PropertyPath(OpacityProperty));
                growStoryboard.Children.Add(fadeInAnnimation);

                var growAnnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(200)
                };
                Storyboard.SetTargetName(growAnnimation, uniformItem.Name);
                Storyboard.SetTargetProperty(growAnnimation, new PropertyPath("UniformItem.RenderTransform.ScaleTransform.ScaleY"));

                growStoryboard.Children.Add(growAnnimation);

                Resources.Add(string.Format("GrowStoryboard{0}", rowNumber), growStoryboard);

                var annimationHelper = new AnnimationHelper(
                    rowDefinition
                    , uniformItem
                    , growStoryboard);

                _annimationItems.Add(item, annimationHelper);

                rowNumber++;
            }
        }

        void GrowStoryboardCompleted(object sender, EventArgs e)
        {
            _growAnnimationHelper.GrowStoryboard.Stop();

            _growCompletedCallback();
        }

        public void GrowItem(object item, Action completed)
        {
            _growCompletedCallback = completed;
            _annimationItems.TryGetValue(item, out _growAnnimationHelper);
            _growAnnimationHelper.GrowStoryboard.Begin();
            _growAnnimationHelper.UniformItem.Opacity = 1;
            _growAnnimationHelper.RowDefinition.Height = 
                new GridLength(_annimationItems.Values.Max(i => i.UniformItem.ActualHeight));
        }

        public void HideItem(object item)
        {
            AnnimationHelper annimationHelper;
            _annimationItems.TryGetValue(item, out annimationHelper);
            annimationHelper.UniformItem.Opacity = 0;
            annimationHelper.RowDefinition.Height = new GridLength(0);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set
            {
                _controller.Items = value.Cast<object>();
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable),
                typeof (UniformItemsControl),
                new PropertyMetadata(OnItemsSource));

        private static void OnItemsSource(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UniformItemsControl) d).ItemsSource = (IEnumerable)e.NewValue;
        }

        public DataTemplate ItemHeaderTemplate
        {
            get { return (DataTemplate) GetValue(ItemHeaderTemplateProperty); }
            set { SetValue(ItemHeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemHeaderTemplateProperty
            = DependencyProperty.Register(
                "ItemHeaderTemplate"
                , typeof (DataTemplate)
                , typeof (UniformItemsControl)
                , null);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemsTemplate", typeof(DataTemplate), typeof(UniformItemsControl), null);

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                _controller.SelectedItem = value;
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem",
                typeof (object),
                typeof (UniformItemsControl),
                new PropertyMetadata(null, OnItemSelected));

        private static void OnItemSelected(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((UniformItemsControl) d).SelectedItem = e.NewValue;
        }

        private Action _growCompletedCallback;
        private AnnimationHelper _growAnnimationHelper;

        private UniformItem CreateUniformItem(object item, int rowNumber)
        {
            var itemControl = new UniformItem
                                  {
                                      ContentTemplate = ItemTemplate,
                                      HeaderTemplate = ItemHeaderTemplate,
                                      DataContext = item,
                                      ShowHeaderOnly = false
                                  };

            itemControl.SetValue(Grid.RowProperty, rowNumber);
            itemControl.OnHeaderLeftClick += OnItemHeaderClicked;
            return itemControl;
        }

        private void OnItemHeaderClicked(object sender, RoutedEventArgs e)
        {
            SetValue(SelectedItemProperty, ((FrameworkElement)sender).DataContext);
        }

        public override void OnApplyTemplate()
        {
            _layoutRoot = (Grid)GetTemplateChild("InnerGrid");
            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var childConstraint = availableSize.Height / (_layoutRoot.Children.Count - 1);

            if(childConstraint < 75)
            {
                childConstraint = 75;
            }

            foreach (var annimationItem in _annimationItems)
            {
                if(annimationItem.Key != SelectedItem)
                {
                    annimationItem.Value.RowDefinition.Height = new GridLength(childConstraint - 5);
                }
            }

            var measureOverride = base.MeasureOverride(availableSize);
            return measureOverride;
        }

        private class AnnimationHelper
        {
            private readonly RowDefinition _rowDefinition;
            private readonly UniformItem _uniformItem;
            private readonly Storyboard _growStoryboard;

            public AnnimationHelper(
                RowDefinition rowDefinition
                , UniformItem uniformItem
                , Storyboard growStoryboard)
            {
                _rowDefinition = rowDefinition;
                _uniformItem = uniformItem;
                _growStoryboard = growStoryboard;
            }

            public UniformItem UniformItem
            {
                get { return _uniformItem; }
            }

            public Storyboard GrowStoryboard
            {
                get { return _growStoryboard; }
            }

            public RowDefinition RowDefinition
            {
                get { return _rowDefinition; }
            }
        }
    }
}