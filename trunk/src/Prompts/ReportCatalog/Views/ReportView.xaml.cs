using System.Windows.Media;
using Prompts.ReportCatalog.ViewModels.Implementation;

namespace Prompts.ReportCatalog.Views
{
    public partial class ReportView
    {
        public ReportView(ReportCatalogItemViewModel viewModel, Brush textBrush)
        {
            DataContext = viewModel;
            InitializeComponent();
            Label.Foreground = textBrush;
        }
    }
}
