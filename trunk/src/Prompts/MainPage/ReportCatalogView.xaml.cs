using System.Windows;

namespace Prompts.MainPage
{
    public partial class ReportCatalogView
    {
        public ReportCatalogView(UIElement catalogControl)
        {
            InitializeComponent();
            ReportCatalogContainer.Children.Add(catalogControl);
        }
    }
}
