using System.Windows;

namespace Prompts.MainPage
{
    public partial class PromptsView
    {
        public PromptsView(UIElement promptCollectionControl)
        {
            InitializeComponent();
            PromptsContainer.Children.Add(promptCollectionControl);
        }
    }
}
