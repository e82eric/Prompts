using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prompts.Prompting.Controls
{
    [TemplateVisualState(GroupName = CommonStates, Name = MouseOver)]
    [TemplateVisualState(GroupName = CommonStates, Name = Normal)]
    [TemplateVisualState(GroupName = CommonStates, Name = Selected)]
    [TemplateVisualState(GroupName = CommonStates, Name = Drawing)]
    public class UniformItem : Control
    {
        private const string CommonStates = "CommonStates";
        private const string MouseOver = "MouseOver";
        private const string Selected = "Selected";
        private const string Normal = "Normal";
        private const string Drawing = "Drawing";

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate) GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                "HeaderTemplate"
                , typeof (DataTemplate)
                , typeof (UniformItem)
                , null);

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate) GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(
                "ContentTemplate"
                , typeof (DataTemplate)
                , typeof (UniformItem)
                , null);

        public object HeaderClicked { get; set; }

        public DependencyProperty HeaderClickedProperty
            = DependencyProperty.Register(
                "ContentTemplate"
                , typeof (DataTemplate)
                , typeof (UniformItem)
                , null);

        public bool ShowHeaderOnly { get; set; }

        public DependencyProperty ShowHeaderOnlyProperty
            = DependencyProperty.Register(
                "ShowHeaderOnly"
                , typeof(bool)
                , typeof(UniformItem)
                , null);

        public RoutedEventHandler OnHeaderLeftClick;

        public UniformItem()
        {
            DefaultStyleKey = typeof (UniformItem);
        }

        private void UniformItemMouseLeave(object sender, MouseEventArgs e)
        {
            GoToState(Normal);
        }

        private void UniformItemMouseEnter(object sender, MouseEventArgs e)
        {
            GoToState(MouseOver);
        }

        private void GoToState(string name)
        {
            VisualStateManager.GoToState(this, name, false);
        }

        public override void OnApplyTemplate()
        {
            GoToState(Drawing);
            base.OnApplyTemplate();
            ReSize();

            var content = (ContentControl)GetTemplateChild("Content");
            content.Content = ContentTemplate.LoadContent();
            var headerContent = (ContentControl) GetTemplateChild("HeaderContent");
            headerContent.Content = HeaderTemplate.LoadContent();
            headerContent.MouseLeftButtonUp += HeaderContentMouseLeftButtonUp;
            headerContent.MouseEnter += UniformItemMouseEnter;
            headerContent.MouseLeave += UniformItemMouseLeave;
        }

        private void ReSize()
        {
            var contentRow = (RowDefinition)GetTemplateChild("ContentRow");
            contentRow.Height = new GridLength(100, GridUnitType.Star);
            var headerRow = (RowDefinition)GetTemplateChild("HeaderRow");
            headerRow.Height = GridLength.Auto;
        }

        private void HeaderContentMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var handler = OnHeaderLeftClick;
            if(handler != null)
            {
                handler(sender, e);
            }
        }
    }
}