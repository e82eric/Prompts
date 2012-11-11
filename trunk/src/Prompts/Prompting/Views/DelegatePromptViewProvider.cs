using System;
using System.Windows.Controls;

namespace Prompts.Prompting.Views
{
    public class DelegatePromptViewProvider : IPromptViewProvider
    {
        private readonly Func< UserControl> _del;

        public DelegatePromptViewProvider(Func<UserControl> del)
        {
            _del = del;
        }

        public UserControl Get(object viewModel)
        {
            var control = _del();
            control.DataContext = viewModel;
            return control;
        }
    }
}
