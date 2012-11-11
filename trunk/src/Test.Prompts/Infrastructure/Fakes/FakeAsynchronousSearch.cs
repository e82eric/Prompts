using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakeAsynchronousSearch
    {
        private readonly Mock<IAsynchronousSearch> _mockAsynchronousSearch;
        private Action<ObservableCollection<ISearchablePromptItem>> _searchCallback;

        public FakeAsynchronousSearch()
        {
            _mockAsynchronousSearch = new Mock<IAsynchronousSearch>();
        }

        public void SetupExecute(
            Action<ObservableCollection<ISearchablePromptItem>> callback,
            Action<string> errorCallback)
        {
            _searchCallback = null;

            var setup =
                _mockAsynchronousSearch.Setup(s => s.Execute(callback, errorCallback));

            setup.Callback(
                (Action<ObservableCollection<ISearchablePromptItem>> c, Action<string> e) =>
            {
                _searchCallback = c;
            });
        }

        public void ExecuteCallback(ObservableCollection<ISearchablePromptItem> promptItems)
        {
            _searchCallback(promptItems);
        }

        public IAsynchronousSearch Object
        {
            get { return _mockAsynchronousSearch.Object; }
        }
    }
}
