using System;
using System.Collections.Generic;
using Moq;
using Prompts.ReportCatalog.Model;

namespace Test.Prompts.Infrastructure.Fakes
{
    public class FakeReportCatalogServiceClient
    {
        private readonly Mock<IReportCatalogServiceClient> _mock;
        private Action<IEnumerable<CatalogItemInfo>> _callBack;
        private Action<string > _errorCallBack;

        public FakeReportCatalogServiceClient()
        {
            _mock = new Mock<IReportCatalogServiceClient>();

            _mock
                .Setup(m => m.GetReportCatalogInfoAsync(
                    It.IsAny<Action<IEnumerable<CatalogItemInfo>>>(),
                    It.IsAny<Action<string>>()))
                .Callback<
                    Action<IEnumerable<CatalogItemInfo>>,
                    Action<string>>((callBack, errorCallback) =>
                        {
                            _callBack = callBack;
                            _errorCallBack = errorCallback;
                        });
        }

        public void ExecuteGetReportCatalogInfoCallback(IEnumerable<CatalogItemInfo> reportCatalogInfo)
        {
            _callBack(reportCatalogInfo);
        }

        public void ExecuteErrorGetReportCatalogInfoCallback(string errorMessage)
        {
            _errorCallBack(errorMessage);
        }

        public IReportCatalogServiceClient Object { get { return _mock.Object; } }
        public Mock<IReportCatalogServiceClient> Mock { get { return _mock; } }
    }
}
