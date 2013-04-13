using System.Windows.Media;
using Prompts.ReportCatalog.ViewModels.Implementation;

namespace Prompts.ReportCatalog.Views
{
    public partial class FolderView
    {
        public FolderView(FolderCatalogItemViewModel viewModel, Brush textBrush)
        {
            DataContext = viewModel;
            InitializeComponent();
            FolderLabel.Foreground = textBrush;
        }
    }
}
