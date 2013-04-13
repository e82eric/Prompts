using System.Windows;
using System.Windows.Controls;

namespace Prompts.Prompting.Views
{
    public partial class PromptTileDataTemplateSelector
    {
        private readonly PromptViewProvider _promptViewProvider;

        public PromptTileDataTemplateSelector()
        {
            _promptViewProvider = new PromptViewProvider(
                new DelegatePromptViewProvider(() => FormatStretch(new SelectedItemsCart())),
                new DelegatePromptViewProvider(() => FormatStretch(new SelectedItemsCart())),
                new DelegatePromptViewProvider(() => FormatStretch(new SelectedItemsCart())),
                new DelegatePromptViewProvider(() => FormatStretch(new DropDownView())),
                new DelegatePromptViewProvider(() => FormatStretch(new SingleSelectTreeView())),
                new DelegatePromptViewProvider(() => FormatStretch(new EmptyPromptView()))); ;
            InitializeComponent();
            Loaded += PromptTileDataTemplateSelectorLoaded;
        }

        void PromptTileDataTemplateSelectorLoaded(object sender, RoutedEventArgs e)
        {
            Content = _promptViewProvider.Get(DataContext);

            Loaded -= PromptTileDataTemplateSelectorLoaded;
        }

        private static UserControl FormatStretch(UserControl control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            return control;
        }
    }
}
