using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.PromptServiceProxy;
using Prompts.Service.PromptService;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakePromptSelectionServiceClient
    {
        private readonly Mock<IPromptSelectionServiceClient> _mock;
        private Action<string> _callback;
        private Action<string> _errorCallback
            ;

        public FakePromptSelectionServiceClient()
        {
            _mock = new Mock<IPromptSelectionServiceClient>();
        }

        public void SetupSetPromptSelections(string path, ObservableCollection<PromptSelectionInfo> promptSelections)
        {
            var setup = _mock.Setup(m => m.SetPromptSelectionsAsync(
                path,
                promptSelections,
                It.IsAny<Action<string>>(),
                It.IsAny<Action<string>>()));

            setup.Callback<
                string,
                ObservableCollection<PromptSelectionInfo>,
                Action<string>,
                Action<string>>((p, ps, response, errorCallback) => 
                { 
                    _callback = response;
                    _errorCallback = errorCallback;
                });
        }

        public void CallbackWith(string response)
        {
            _callback(response);
        }

        public void ErrorCallbackWith(string errorMessage)
        {
            _errorCallback(errorMessage);
        }

        public IPromptSelectionServiceClient Object
        {
            get { return _mock.Object; }
        }
    }
}
