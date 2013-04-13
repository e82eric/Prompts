using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.ReportCatalog.Model;
using Prompts.ReportCatalog.ViewModels.Implementation;

namespace Test.Prompts.ReportCatalog.ViewModels.Implementation
{
    [TestClass]
    public class ReportCatalogItemViewModelTest
    {
        private ReportCatalogItemViewModel _reportCatalogItemViewModel;
        private CatalogItemInfo _catalogItemInfo;
        private Mock<IPromptsViewModel> _promptsViewModel;

        [TestInitialize]
        public void Setup()
        {
            _promptsViewModel = new Mock<IPromptsViewModel>();
            _catalogItemInfo = new CatalogItemInfo {Name = "Name", Path = "Path", Type = CatalogItemType.Report};
            _reportCatalogItemViewModel = new ReportCatalogItemViewModel(_catalogItemInfo, _promptsViewModel.Object);
        }

        [TestMethod]
        public void UsesCorrectFieldsFromInfoObject()
        {
            Assert.AreEqual(_catalogItemInfo.Name, _reportCatalogItemViewModel.ReportName);
        }

        [TestMethod]
        public void CallsRenderMethodOnThePromptsViewModelWhenShowPromptCollectionIsExecuted()
        {
            _reportCatalogItemViewModel.ShowPromptCollection.Execute(null);

            _promptsViewModel.Verify(m => m.ShowPromptsFor(_catalogItemInfo), Times.Exactly(1));
        }
    }
}