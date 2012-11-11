using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.ReportCatalog.Model;
using Prompts.ReportCatalog.ViewModels;
using Prompts.ReportCatalog.ViewModels.Implementation;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.ReportCatalog.ViewModels.Implementation
{
    [TestClass]
    public class ReportCatalogViewModelServiceTest
    {
        private ReportCatalogViewModelService _catalogViewModelService;
        private FakeReportCatalogServiceClient _reportCatalogServiceClient;
        private Mock<ICatalogItemViewModelBuilder> _reportCatalogBuilder;

        [TestInitialize]
        public void Setup()
        {
            _reportCatalogServiceClient = new FakeReportCatalogServiceClient();
            _reportCatalogBuilder = new Mock<ICatalogItemViewModelBuilder>();

            _catalogViewModelService = new ReportCatalogViewModelService(
                _reportCatalogServiceClient.Object,
                _reportCatalogBuilder.Object);
        }

        [TestMethod]
        public void CallsBackWithReportCatalogWhenSucesscful()
        {
            var numberOfCallBacks = 0;
            var numberOfErrorCallbacks = 0;

            var itemInfo1 = A.CatalogItemInfo().WithName("Item 1").Build();
            var itemInfo2 = A.CatalogItemInfo().WithName("Item 2").Build();
            var itemInfos = A.ObservableCollection(itemInfo1, itemInfo2);

            var catalogItemsFromBuilder = A.ObservableCollection(Mock.Of<ICatalogItemViewModel>(), Mock.Of<ICatalogItemViewModel>());

            _reportCatalogBuilder.Setup(b => b.BuildFrom(itemInfos)).Returns(catalogItemsFromBuilder);

            _catalogViewModelService.GetReportCatalog(c =>
                {
                    c.AssertEquals(catalogItemsFromBuilder);
                    numberOfCallBacks++;
                },
                e => { numberOfErrorCallbacks++; });

            _reportCatalogServiceClient
                .Mock.Verify(c => c.GetReportCatalogInfoAsync(
                    It.IsAny<Action<IEnumerable<CatalogItemInfo>>>(),
                    It.IsAny<Action<string>>()), Times.Exactly(1));

            Assert.AreEqual(0, numberOfCallBacks);
            Assert.AreEqual(0, numberOfErrorCallbacks);

            _reportCatalogServiceClient.ExecuteGetReportCatalogInfoCallback(itemInfos);

            Assert.AreEqual(1, numberOfCallBacks);
            Assert.AreEqual(0, numberOfErrorCallbacks);
        }

        [TestMethod]
        public void RaisesEventWhenClientServiceIndicatesThatAnErrorOccurs()
        {
            var numberOfCallBacks = 0;
            var numberOfErrorCallbacks = 0;

            const string errorMessage = "Error Message";

            _catalogViewModelService.GetReportCatalog(c => { numberOfCallBacks++; }, e => { numberOfErrorCallbacks++; });

            _reportCatalogServiceClient.Mock.Verify(
                c => c.GetReportCatalogInfoAsync(
                    It.IsAny<Action<IEnumerable<CatalogItemInfo>>>(),
                    It.IsAny<Action<string>>()), Times.Exactly(1));

            Assert.AreEqual(0, numberOfCallBacks);
            Assert.AreEqual(0, numberOfErrorCallbacks);

            _reportCatalogServiceClient.ExecuteErrorGetReportCatalogInfoCallback(errorMessage);

            Assert.AreEqual(0, numberOfCallBacks);
            Assert.AreEqual(1, numberOfErrorCallbacks);
        }
    }
}