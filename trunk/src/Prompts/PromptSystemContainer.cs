using System;
using System.Windows.Controls;
using Prompts.Prompting.Views;
using Prompts.ReportRendering.Implementation;
using Prompts.ReportRendering.ViewModel;

namespace Prompts
{
    public class PromptSystemContainer
    {
        private IReportRenderer _reportRenderer;
        private PromptCollectionControl _promptCollectionControl;
        private UserControl _promptsControl;

        public PromptSystemContainer()
        {
            ReportCatalogContainer = new ReportCatalogContainer();
            PromptContainer = new PromptContainer();
            ReportRendererContainer = new ReportRendererContainer();
        }

        public ReportCatalogContainer ReportCatalogContainer { get; set; }
        public PromptContainer PromptContainer { get; set; }
        public ReportRendererContainer ReportRendererContainer { get; set; }

        private object CreateReportRenderer()
        {
            if (_reportRenderer == null)
            {
                _reportRenderer = ReportRendererContainer.Create();
            }

            return _reportRenderer;
        }

        public object CreatePopupView()
        {
            CreateReportRendererIfItIsNull();
            var popupReortViewModel = _reportRenderer as IPopupReportViewModel;

            if (popupReortViewModel == null)
            {
                throw new Exception(
                    "Unable to Create Popup View:  Report Renderer does not implement IPopupReportViewModel");
            }

            return new PopupReportView(popupReortViewModel);
        }


        public UserControl CreatePromptCollectionControl()
        {
            if (_promptCollectionControl != null)
            {
                return _promptCollectionControl;
            }

            CreateReportRendererIfItIsNull();
            PromptContainer.ReportRenderer = _reportRenderer;
            _promptCollectionControl = PromptContainer.Create();
            return _promptCollectionControl;
        }

        public UserControl CreateReportCatalogControl()
        {
            if (_promptsControl == null)
            {
                _promptsControl = CreatePromptCollectionControl();
            }
            ReportCatalogContainer.PromptsViewModel = PromptContainer.PromptsViewModel;
            return ReportCatalogContainer.Create();
        }

        private void CreateReportRendererIfItIsNull()
        {
            if (_reportRenderer == null)
            {
                _reportRenderer = ReportRendererContainer.Create();
            }
        }
    }
}
