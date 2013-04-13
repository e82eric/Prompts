using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.ReportCatalog.ViewModels;

namespace Test.Prompts.Infrastructure.Fakes
{
    public class FakeReportCatalogViewModelService
    {
        private readonly Mock<IReportCatalogViewModelService> _mock;
        private Action<ObservableCollection<ICatalogItemViewModel>> _callback;
        private Action<string> _errorCallback;

        public FakeReportCatalogViewModelService()
        {
            _mock = new Mock<IReportCatalogViewModelService>();

            _callback = null;
            var setup = _mock.Setup(
                m => m.GetReportCatalog(
                    It.IsAny<Action<ObservableCollection<ICatalogItemViewModel>>>(),
                    It.IsAny<Action<string>>()));

            setup.Callback<Action<ObservableCollection<ICatalogItemViewModel>>, Action<string>>((callback, errorCallback) =>
            {
                _callback = callback;
                _errorCallback = errorCallback;
            });
        }

        public void ExecuteGetReportCatalogCallback(ObservableCollection<ICatalogItemViewModel> catalogItems)
        {
            _callback(catalogItems);
        }

        public IReportCatalogViewModelService Object
        {
            get { return _mock.Object; }
        }

        public void ExecuteErrorCallback(string errorMessage)
        {
            _errorCallback(errorMessage);
        }
    }
}
