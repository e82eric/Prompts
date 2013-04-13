using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.ViewModels.Search.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Search.Implementation
{
    [TestClass]
    public class AsynchronousSearchTest
    {
        private const string PromptName = "Prompt Name";
        private const string ParameterName = "Parameter Name";
        private Mock<ISearch> _search;
        private AsynchronousSearch _asynchronousSearch;
        private ChildPromptItemServiceHelper _childPromptItemServiceHelper;

        [TestInitialize]
        public void Setup()
        {
            _childPromptItemServiceHelper = new ChildPromptItemServiceHelper();
            _search = new Mock<ISearch>();

            _asynchronousSearch = new AsynchronousSearch(
                PromptName
                , ParameterName
                , _childPromptItemServiceHelper.Object
                , _search.Object);
        }

        [TestMethod]
        public void ItSendsTheCorrectParameterValueToThePromptService2()
        {
            var numberOfCallbacks = 0;

            const string searchExpression = "Search Expression";

            var childItemsToCallbackWith = new Mock<ISearchablePromptItemCollection>().Object;
            var searchResults = A.ObservableCollection(Mock.Of<ISearchablePromptItem>());

            _search.SetupGet(r => r.SearchString).Returns(searchExpression);
            _search.Setup(r => r.Execute(childItemsToCallbackWith)).Returns(searchResults);

            _childPromptItemServiceHelper.SetupGetChildren(PromptName, ParameterName, searchExpression);

            _asynchronousSearch.Execute(
                r =>
                    {
                        numberOfCallbacks++;
                        Assert.AreEqual(searchResults, r);
                    },
                e => { });

            _childPromptItemServiceHelper.ExecuteSetupGetChildrenCallback(childItemsToCallbackWith);

            Assert.AreEqual(1, numberOfCallbacks);
        }

        private class ChildPromptItemServiceHelper
        {
            private readonly Mock<IChildPromptItemsService> _mockChildPromptItemService;
            private Action<ISearchablePromptItemCollection> _callback;

            public ChildPromptItemServiceHelper()
            {
                _mockChildPromptItemService = new Mock<IChildPromptItemsService>();
            }

            public IChildPromptItemsService Object
            {
                get { return _mockChildPromptItemService.Object; }
            }

            public void SetupGetChildren(
                string promptName
                , string parameterName
                , string value)
            {
                var setup = _mockChildPromptItemService.Setup(
                    s =>
                    s.GetChildren(
                        promptName,
                        parameterName,
                        value,
                        It.IsAny<Action<ISearchablePromptItemCollection>>(),
                        It.IsAny<Action<string>>()));

                setup.Callback((
                    string n, 
                    string pn, 
                    string v, 
                    Action<ISearchablePromptItemCollection> c,
                    Action<string> e) =>
                {
                    _callback = c;
                });
            }

            public void ExecuteSetupGetChildrenCallback(ISearchablePromptItemCollection promptItemCollection)
            {
                _callback(promptItemCollection);
            }
        }
    }
}
