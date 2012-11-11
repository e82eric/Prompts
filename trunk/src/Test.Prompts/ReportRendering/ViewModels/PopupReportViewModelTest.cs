using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.ReportRendering.ViewModel;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.ReportRendering.ViewModels
{
    [TestClass]
    public class PopupReportViewModelTest
    {
        private Mock<IReportViewModelBuilder> _reportViewModelBuilder;
        private PopupReportViewModel _popupReportViewModel;

        [TestInitialize]
        public void Setup()
        {
            _reportViewModelBuilder = new Mock<IReportViewModelBuilder>();
            _popupReportViewModel = new PopupReportViewModel(_reportViewModelBuilder.Object);
        }

        [TestMethod]
        public void ReportViewModelIsAddedToCollectionCorrectly()
        {
            var numberOfReportEvents = 0;

            var catalogItemInfo = A.CatalogItemInfo().Build();
            var selectionInfos = A.ObservableCollection(A.PromptSelectionInfo().Build());

            var reportViewModelForBuilderToReturn = new Mock<IReportViewModel>().Object;

            _reportViewModelBuilder.Setup(b => b.Build(catalogItemInfo, selectionInfos)).Returns(
                reportViewModelForBuilderToReturn);

            _popupReportViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "Report")
                    {
                        numberOfReportEvents++;
                    }
                };

            Assert.AreEqual(0, numberOfReportEvents);
            Assert.IsNull(_popupReportViewModel.Report);

            _popupReportViewModel.AddReport(catalogItemInfo, selectionInfos);

            Assert.AreEqual(1, numberOfReportEvents);
            Assert.AreEqual(reportViewModelForBuilderToReturn, _popupReportViewModel.Report);
        }
    }
}
