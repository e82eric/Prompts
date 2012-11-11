using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Prompting.Views;
using Prompts.PromptServiceProxy;
using Prompts.ReportRendering.ViewModel;

namespace Prompts
{
    public class PromptContainer
    {
        public IReportRenderer ReportRenderer { get; set; }

        public MultiSelectTreeContainer MultiSelectTreeContainer { get; set; }
        public DropDownContainer DropDownContainer { get; set; }
        public CasscadingSearchContainer CasscadingSerachContainer { get; set; }
        public ShoppingCartContainer ShoppingCartContainer { get; set; }
        public EmptyPromptContainer EmptyPromptContainer { get; set; }
        public SingleSelectTreeContainer SingleSelectTreeContainer { get; set; }

        public IPromptsViewModel PromptsViewModel { get; set; }

        public Color TextColor { get; set; }

        public PromptContainer()
        {
            TextColor = Colors.Black;
            MultiSelectTreeContainer = new MultiSelectTreeContainer();
            DropDownContainer = new DropDownContainer();
            CasscadingSerachContainer = new CasscadingSearchContainer();
            ShoppingCartContainer = new ShoppingCartContainer();
            EmptyPromptContainer = new EmptyPromptContainer();
            SingleSelectTreeContainer = new SingleSelectTreeContainer();
        }

        public PromptCollectionControl Create()
        {
            PromptsViewModel = CreatePromptsViewModel();

            return CreatePromptsView(PromptsViewModel);
        }

        protected virtual PromptCollectionControl CreatePromptsView(IPromptsViewModel promptsViewModel)
        {
            return new PromptCollectionControl(promptsViewModel, new SolidColorBrush(TextColor));
        }

        private IPromptsViewModel CreatePromptsViewModel()
        {
            var promptsViewModelService = CreatePromptsViewModelService();

            return new PromptsViewModel(promptsViewModelService, ReportRenderer);
        }

        private IPromptsViewModelService CreatePromptsViewModelService()
        {
            const string absoluteServiceUri = "/Prompts.Service/api/Prompts";
            string uri;
            if (Application.Current.Host.Source != null)
            {
                uri = new Uri(Application.Current.Host.Source, absoluteServiceUri).AbsoluteUri;
            }
            else
            {
                throw new Exception(
                    "An excpetion occured while trying to resolve 'Application.Current.Host.Source'");
            }

            return new PromptsViewModelService(
                new PromptsViewModelBuilder(
                    new PromptBuilder(
                        MultiSelectTreeContainer.Create(),
                        DropDownContainer.Create(),
                        ShoppingCartContainer.Create(),
                        CasscadingSerachContainer.Create(),
                        SingleSelectTreeContainer.Create(),
                        EmptyPromptContainer.Create(),
                        new RecursiveTreeContainer().Create(),
                        new RecursiveSingleSelectTreeContainer().Create())),
                ServiceInjector.Inject<IPromptServiceClient>());
        }

        protected virtual IPromptsViewModel CreatePromptsViewModel(
            IPromptsViewModelService promptsViewModelService,
            IReportRenderer reportRenderer)
        {
            return new PromptsViewModel(promptsViewModelService, reportRenderer);
        }

        public virtual PromptViewProvider CreatePromptViewProvider()
        {
            return new PromptViewProvider(
                new DelegatePromptViewProvider(() => FormatStretch(new ShoppingCartTreeView())),
                new DelegatePromptViewProvider(() => FormatStretch(new ShoppingCartView())),
                new DelegatePromptViewProvider(() => FormatStretch(new CasscadingSearchView())),
                new DelegatePromptViewProvider(() => FormatDropDown(new DropDownView())),
                new DelegatePromptViewProvider(() => FormatDropDown(new SingleSelectTreeView())),
                new DelegatePromptViewProvider(() => FormatStretch(new EmptyPromptView())));
        }

        public virtual PromptViewProvider CreatePromptTileViewProvider()
        {
            return new PromptViewProvider(
                new DelegatePromptViewProvider(() => FormatStretch(new SelectedItemsCart())),
                new DelegatePromptViewProvider(() => FormatStretch(new SelectedItemsCart())),
                new DelegatePromptViewProvider(() => FormatStretch(new SelectedItemsCart())),
                new DelegatePromptViewProvider(() => FormatStretch(new DropDownView())),
                new DelegatePromptViewProvider(() => FormatStretch(new SingleSelectTreeView())),
                new DelegatePromptViewProvider(() => FormatStretch(new EmptyPromptView())));
        }

        private static UserControl FormatStretch(UserControl control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            return control;
        }

        private static UserControl FormatDropDown(UserControl control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Left;
            control.Width = 300;
            control.Margin = new Thickness(5);
            return control;
        }
    }
}
