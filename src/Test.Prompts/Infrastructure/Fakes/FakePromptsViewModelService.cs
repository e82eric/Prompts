using System;
using System.Collections.Generic;
using Moq;
using Prompts.Prompting.ViewModels;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakePromptsViewModelService
    {
        private readonly Mock<IPromptsViewModelService> _promptsViewModelService;
        private Action<IEnumerable<IPrompt>> _callBack;
        private Action<string> _errorCallback;

        public FakePromptsViewModelService()
        {
            _promptsViewModelService = new Mock<IPromptsViewModelService>();
        }

        public IPromptsViewModelService Object
        {
            get { return _promptsViewModelService.Object; }
        }

        public void SetupGetPrompts(string reportName)
        {
            var setup = _promptsViewModelService
                .Setup(
                s => s.GetPromptViewModels(
                    reportName,
                    It.IsAny<Action<IEnumerable<IPrompt>>>(),
                    It.IsAny<Action<string>>()));

            setup.Callback<
                string, 
                Action<IEnumerable<IPrompt>>,
                Action<string>>(
                    (n, callback, errorCallback) =>
                    {
                        _callBack = callback;
                        _errorCallback = errorCallback;
                    });
        }

        public void ExecuteCallback(IEnumerable<IPrompt> prompts)
        {
            _callBack(prompts);
        }

        public void AssertNumberOfGetPrompts(string name, Times exactly)
        {
            _promptsViewModelService
                .Verify
                    (s => s.GetPromptViewModels(
                        name,
                        It.IsAny<Action<IEnumerable<IPrompt>>>(),
                        It.IsAny<Action<string>>())
                    , exactly);
        }

        public void AssertNumberOfCancels(string reportPath, Times times)
        {
            _promptsViewModelService.Verify(s => s.CancelGetPromptViewModels(reportPath), times);
        }

        public void ExecuteErrorCallback(string errorMessage)
        {
            _errorCallback(errorMessage);
        }
    }
}
