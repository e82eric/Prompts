using System.Windows.Controls;

namespace Prompts.Prompting.Views
{
    public interface IPromptViewProvider
    {
        UserControl Get(object viewModel);
    }
}