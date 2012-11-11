using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakeAsynchronousSearchService
    {
        private readonly Mock<IAsynchronousSearchService> _mock;
        private Action<ObservableCollection<ISearchablePromptItem>> _callback;
        private Action<string> _errorCallback;

        public FakeAsynchronousSearchService()
        {
            _mock = new Mock<IAsynchronousSearchService>();
        }

        public IAsynchronousSearchService Object
        {
            get { return _mock.Object; }
        }

        public void SetupSearch(string searchExpression)
        {
            var setup = _mock.Setup(
                s =>
                s.Search(
                    searchExpression,
                    It.IsAny<Action<ObservableCollection<ISearchablePromptItem>>>(),
                    It.IsAny<Action<string>>()));

            setup.Callback((string s, Action<ObservableCollection<ISearchablePromptItem>> c, Action<string> e) =>
                {
                    _callback = c;
                    _errorCallback = e;
                });
        }

        public void ExecuteSearchCallback(ObservableCollection<ISearchablePromptItem> promptItemCollection)
        {
            _callback(promptItemCollection);
        }

        public void ExecuteErrorCallback(string errorMessage)
        {
            _errorCallback(errorMessage);
        }
    }
}
