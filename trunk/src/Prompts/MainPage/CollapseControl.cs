using System.Windows;
using System.Windows.Controls;

namespace Prompts.MainPage
{
    public class CollapseControl : Grid
    {
        public UserControl CollapseableContent
        {
            get { return (UserControl)GetValue(CollapseableContentProperty); }
            set { SetValue(CollapseableContentProperty, value); }
        }

        public static DependencyProperty CollapseableContentProperty =
            DependencyProperty.Register(
                "CollapseableContent"
                , typeof(UserControl)
                , typeof (CollapseControl)
                , new PropertyMetadata(OnCollapseableContent));

        private static void OnCollapseableContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateIfInitalized(d);
        }

        private static void CreateIfInitalized(DependencyObject d)
        {
            var control = (CollapseControl) d;

            if(IsControlInitalized(control))
            {
                control.Create();
            }
        }

        private static bool IsControlInitalized(CollapseControl control)
        {
            return control.CollapseableContent != null && control.StaticContent != null;
        }

        public UserControl StaticContent
        {
            get { return (UserControl)GetValue(StaticContentProperty); }
            set { SetValue(StaticContentProperty, value); }
        }

        public static DependencyProperty StaticContentProperty =
            DependencyProperty.Register(
                "StaticContent"
                , typeof(UserControl)
                , typeof(CollapseControl)
                , new PropertyMetadata(OnStaticContent));

        private static void OnStaticContent(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateIfInitalized(d);
        }

        public bool IsCollapsed
        {
            get { return (bool) GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value);}
        }

        public static DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(
                "IsCollapsed"
                , typeof (bool)
                , typeof (CollapseControl)
                , new PropertyMetadata(OnIsCollapseable));

        private static void OnIsCollapseable(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (CollapseControl) d;

            if(IsControlInitalized(control))
            {
                if((bool)e.NewValue)
                {
                    control.Collapse();
                }
                else
                {
                    control.DeCollapse();
                }
            }
        }

        private ColumnDefinition _collapseableColumn;

        internal void Create()
        {
            RowDefinitions.Add(new RowDefinition {Height = new GridLength(100, GridUnitType.Star)});
            _collapseableColumn = new ColumnDefinition {Width = new GridLength(30, GridUnitType.Auto)};
            var staticColumn = new ColumnDefinition {Width = new GridLength(70, GridUnitType.Star)};

            ColumnDefinitions.Add(_collapseableColumn);
            ColumnDefinitions.Add(staticColumn);

            CollapseableContent.SetValue(ColumnProperty, 0);
            StaticContent.SetValue(ColumnProperty, 1);

            Children.Add(CollapseableContent);
            Children.Add(StaticContent);
        }

        internal void Collapse()
        {
            _collapseableColumn.Width = new GridLength(0, GridUnitType.Star);
        }

        internal void DeCollapse()
        {
            _collapseableColumn.Width = new GridLength(30, GridUnitType.Auto);
        }
    }
}
