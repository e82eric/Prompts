using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.ReportCatalog.Model;
using Prompts.ReportRendering;
using Prompts.Service.PromptService;

namespace Test.Prompts.Infrastructure.Fakes
{
    public class FakeReportExecutionService
    {
        private readonly Mock<IReportExecutionService> _mock;
        private Action<string> _callback;
        private Action<string> _errorCallback;

        public FakeReportExecutionService()
        {
            _mock = new Mock<IReportExecutionService>();
        }

        public IReportExecutionService Object
        {
            get { return _mock.Object; }
        }

        public void SetupRender(
            CatalogItemInfo catalogItemInfo, 
            ObservableCollection<PromptSelectionInfo> promptSelectionInfos)
        {
            var setup = _mock.Setup(
                m => m.Render(
                    catalogItemInfo,
                    promptSelectionInfos,
                    It.IsAny<Action<string>>(),
                    It.IsAny<Action<string>>()));

            setup.Callback<
                CatalogItemInfo, 
                ObservableCollection<PromptSelectionInfo>, 
                Action<string>, 
                Action<string>>(
                (i, s, c, ec) =>
                    {
                        _callback = c;
                        _errorCallback = ec;
                    } );
        }

        public void ExecuteRenderCallback(string executionId)
        {
            _callback(executionId);
        }

        public void ExecuteErrorCallback(string errorMessage)
        {
            _errorCallback(errorMessage);
        }
    }
}
