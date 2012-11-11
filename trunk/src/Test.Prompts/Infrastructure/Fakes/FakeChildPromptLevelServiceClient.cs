using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.Prompting.Model;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakeChildPromptLevelServiceClient
    {
        private readonly Mock<IChildPromptLevelServiceClient> _childTreeNodeService;
        private Action<PromptLevel> _callback;
        private Action<string> _errorCallback;

        public FakeChildPromptLevelServiceClient()
        {
            _childTreeNodeService = new Mock<IChildPromptLevelServiceClient>();
        }

        public void SetupGetChildren2(
            string promptName,
            string parameterName,
            string value)
        {
            var setup = _childTreeNodeService
                .Setup(
                    s => s.GetChildren2Async(
                        promptName
                        , parameterName
                        , It.Is<ParameterValue>(v => v.Value == value)
                        , It.IsAny<Action<PromptLevel>>()
                        , It.IsAny<Action<string>>()));

            setup.Callback<
                string,
                string,
                ParameterValue,
                Action<PromptLevel>,
                 Action<string>>((s, s2, pv, callback, errorCallback) =>
                     {
                         _callback = callback;
                         _errorCallback = errorCallback;
                     });
        }

        public void SetupGetChildren(
            string promptName,
            string parameterName, 
            ObservableCollection<ParameterValue> parameterValues)
        {
            var setup = _childTreeNodeService
                .Setup(
                    s => s.GetChildrenAsync(
                        promptName
                        , parameterName
                        , parameterValues
                        , It.IsAny<Action<PromptLevel>>()
                        , It.IsAny<Action<string>>()));

            setup.Callback<
                string,
                string,
                ObservableCollection<ParameterValue>,
                Action<PromptLevel>,
                Action<string>>(
                    (s, s2, pv, callback, errorCallback) =>
                        {
                            _callback = callback;
                            _errorCallback = errorCallback;
                        });
        }

        public void RaiseGetChildren2Completed(PromptLevel getChildrenResponse)
        {
            _callback(getChildrenResponse);
        }

        public void RaiseGetChildrenCompleted(PromptLevel getChildrenResponse)
        {
            _callback(getChildrenResponse);
        }

        public void RaiseGetChildrenError(string errorMessage)
        {
            _errorCallback(errorMessage);
        }

        public IChildPromptLevelServiceClient Object
        {
            get { return _childTreeNodeService.Object; }
        }
    }
}
