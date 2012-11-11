using System.Windows.Controls;
using Prompts.Prompting.ViewModels.Implementation;

namespace Prompts.Prompting.Views
{
    public partial class EmptyPromptView
    {
        public EmptyPromptView()
        {
            InitializeComponent();
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            ((EmptyPrompt) DataContext).Text = TextBoxInput.Text;
        }
    }
}
