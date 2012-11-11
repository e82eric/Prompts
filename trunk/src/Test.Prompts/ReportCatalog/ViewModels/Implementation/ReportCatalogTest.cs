using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Infastructure;
using Prompts.ReportCatalog.ViewModels;
using Prompts.ReportCatalog.ViewModels.Implementation;
using Test.Prompts.Infrastructure;
using Test.Prompts.Infrastructure.Fakes;

namespace Test.Prompts.ReportCatalog.ViewModels.Implementation
{
    [TestClass]
    public class ReportCatalogTest
    {
        private FakeReportCatalogViewModelService _reportCatalogViewModelService;
        private ReportCatalogViewModel _catalog;

        [TestInitialize]
        public void Setup()
        {
            _reportCatalogViewModelService = new FakeReportCatalogViewModelService();
            _catalog = new ReportCatalogViewModel(_reportCatalogViewModelService.Object);
        }

        [TestMethod]
        public void ItemsAreSetToWhatServiceReturns()
        {
            var itemsToCallBackWith = A.ObservableCollection(
                Mock.Of<ICatalogItemViewModel>(),
                Mock.Of<ICatalogItemViewModel>());

            AssertRefreshFromService(itemsToCallBackWith);
        }

        [TestMethod]
        public void ItRequeriesTheServiceWhenReconnectIsExecuted()
        {
            var catalogItemsToCallBackWith = A.ObservableCollection(Mock.Of<ICatalogItemViewModel>());

            var catalogItemsToReconnectWith = A.ObservableCollection(
                Mock.Of<ICatalogItemViewModel>(),
                Mock.Of<ICatalogItemViewModel>());

            _reportCatalogViewModelService.ExecuteGetReportCatalogCallback(catalogItemsToCallBackWith);

            _catalog.Reconnect.Execute(null);

            AssertRefreshFromService(catalogItemsToReconnectWith);
        }

        [TestMethod]
        public void StateChangesFromLoadingToErrorWhenSerivceRaisesErrorEvent()
        {
            AssertErrorMessageWhenServiceCallsbackWIthError();
        }

        [TestMethod]
        public void StateChangesFromLoadingToErrorWhenSerivceRaisesErrorEventDuringReconnect()
        {
            _reportCatalogViewModelService.ExecuteGetReportCatalogCallback(A.ObservableCollection(Mock.Of<ICatalogItemViewModel>()));

            _catalog.Reconnect.Execute(null);

            AssertErrorMessageWhenServiceCallsbackWIthError();
        }

        private void AssertRefreshFromService(ObservableCollection<ICatalogItemViewModel> catalogItemsToCallBackWith)
        {
            var numberOfItemsEvents = 0;
            var numberOfStateEvents = 0;

            PropertyChangedEventHandler catalogOnPropertyChanged = (s, e) =>
                {
                    if (e.PropertyName.Equals("Items"))
                    {
                        numberOfItemsEvents++;
                    }
                    if (e.PropertyName.Equals("State"))
                    {
                        numberOfStateEvents++;
                    }
                };

            _catalog.PropertyChanged += catalogOnPropertyChanged;

            Assert.AreEqual(0, numberOfStateEvents);
            Assert.AreEqual(0, numberOfItemsEvents);
            Assert.AreEqual(ViewModelState.Loading, _catalog.State);
            _catalog.Items.AssertLength(0);

            _reportCatalogViewModelService.ExecuteGetReportCatalogCallback(catalogItemsToCallBackWith);

            Assert.AreEqual(1, numberOfItemsEvents);
            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(catalogItemsToCallBackWith, _catalog.Items);
            Assert.AreEqual(ViewModelState.Loaded, _catalog.State);

            _catalog.PropertyChanged -= catalogOnPropertyChanged;
        }

        private void AssertErrorMessageWhenServiceCallsbackWIthError()
        {
            var numberOfItemsEvents = 0;
            var numberOfErrorMessageEvents = 0;
            var numberOfStateEvents = 0;

            PropertyChangedEventHandler catalogOnPropertyChanged = (s, e) =>
                {
                    if (e.PropertyName.Equals("Items"))
                    {
                        numberOfItemsEvents++;
                    }
                    if (e.PropertyName.Equals("State"))
                    {
                        numberOfStateEvents++;
                    }
                    if (e.PropertyName.Equals("ErrorMessage"))
                    {
                        numberOfErrorMessageEvents++;
                    }
                };

            _catalog.PropertyChanged += catalogOnPropertyChanged;

            Assert.AreEqual(0, numberOfStateEvents);
            Assert.AreEqual(0, numberOfItemsEvents);
            Assert.AreEqual(ViewModelState.Loading, _catalog.State);
            _catalog.Items.AssertLength(0);

            const string expectedErrorMessage = "Error Getting Report Catalog";

            _reportCatalogViewModelService.ExecuteErrorCallback(expectedErrorMessage);

            Assert.AreEqual(0, numberOfItemsEvents);
            Assert.AreEqual(1, numberOfStateEvents);
            Assert.AreEqual(1, numberOfErrorMessageEvents);
            _catalog.Items.AssertLength(0);
            Assert.AreEqual(ViewModelState.Error, _catalog.State);
            Assert.AreEqual(expectedErrorMessage, _catalog.ErrorMessage);

            _catalog.PropertyChanged -= catalogOnPropertyChanged;
        }
    }
}