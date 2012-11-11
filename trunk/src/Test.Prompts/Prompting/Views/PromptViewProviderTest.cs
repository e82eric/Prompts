using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Prompting.ViewModels.Search;
using Prompts.Prompting.Views;

namespace Test.Prompts.Prompting.Views
{
    [TestClass]
    public class PromptViewProviderTest
    {
        private Mock<IPromptViewProvider> _multiSelectHierarchyViewProvider;
        private Mock<IPromptViewProvider> _multiSelectViewProvider;
        private Mock<IPromptViewProvider> _casscadingSearchViewProvider;
        private Mock<IPromptViewProvider> _singleSelectViewProvider;
        private Mock<IPromptViewProvider> _singleSelectHierarchyViewProvider;
        private Mock<IPromptViewProvider> _emptyViewProvider;
        private PromptViewProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            _multiSelectHierarchyViewProvider = new Mock<IPromptViewProvider>();
            _multiSelectViewProvider = new Mock<IPromptViewProvider>();
            _casscadingSearchViewProvider = new Mock<IPromptViewProvider>();
            _singleSelectViewProvider = new Mock<IPromptViewProvider>();
            _singleSelectHierarchyViewProvider = new Mock<IPromptViewProvider>();
            _emptyViewProvider = new Mock<IPromptViewProvider>();

            _provider = new PromptViewProvider(
                _multiSelectHierarchyViewProvider.Object,
                _multiSelectViewProvider.Object,
                _casscadingSearchViewProvider.Object,
                _singleSelectViewProvider.Object,
                _singleSelectHierarchyViewProvider.Object,
                _emptyViewProvider.Object);
        }

        [TestMethod]
        public void ItUsesTheMultiSelectHierarchyProviderWhenTheViewModelIsAMultiSelectHierarchy()
        {
            var viewModel = new MultiSelectHierarchy(
                "name", 
                "label", 
                new ObservableCollection<ITreeNode>(),
                new ObservableCollection<ITreeNode>()); 

            var expected = new UserControl();

            _multiSelectHierarchyViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheMultiSelectHierarchyProviderWhenTheViewModelIsASubClassOfMultiSelectHierarchy()
        {
            var viewModel = new MultiSelectHierarchyTestImpl();

            var expected = new UserControl();

            _multiSelectHierarchyViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheMultiSelectProviderWhenTheViewModelIsAMultiSelect()
        {
            var viewModel = new MultiSelectSearch(
                "name",
                "label",
                Mock.Of<ISearchService>(),
                new ObservableCollection<ISearchablePromptItem>());

            var expected = new UserControl();

            _multiSelectViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheMultiSelectProviderWhenTheViewModelIsASubClassOfMultiSelect()
        {
            var viewModel = new MultiSelectTestingImpl();

            var expected = new UserControl();

            _multiSelectViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheCasscadingSearchProviderWhenTheViewModelIsAMultiSelectCasscadingSearch()
        {
            var viewModel = new MultiSelectCasscadingSearch(
                "label",
                "name",
                Mock.Of<IAsynchronousSearchService>(),
                new ObservableCollection<ISearchablePromptItem>(), 
                new ObservableCollection<ISearchablePromptItem>());

            var expected = new UserControl();

            _casscadingSearchViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheCasscadingSearchProviderWhenTheViewModelIsASubClassOfMultiSelectCasscadingSearch()
        {
            var viewModel = new MultiSelectCasscadingSearchTestImpl();

            var expected = new UserControl();

            _casscadingSearchViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheSingleSelectViewProviderWhenTheViewModelIsASingleSelect()
        {
            var viewModel = new SingleSelectPrompt<ISearchablePromptItem>(
                "name",
                "label",
                new ObservableCollection<ISearchablePromptItem>(),
                null);

            var expected = new UserControl();

            _singleSelectViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheSingleSelectViewProviderWhenTheViewModelIsASubClassOfSingleSelect()
        {
            var viewModel = new SingleSelectTestImpl();
            var expected = new UserControl();

            _singleSelectViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheEmptyViewProviderWhenTheViewModelIsAEmptyPrompt()
        {
            var viewModel = new EmptyPrompt(
                "name",
                "label");

            var expected = new UserControl();

            _emptyViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheEmptyViewProviderWhenTheViewModelIsASubClassOfEmptyPrompt()
        {
            var viewModel = new EmptyPromptTestImmpl();

            var expected = new UserControl();

            _emptyViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ItUsesTheCingleSelectHierararchyViewProviderWhenTheViewModelIsASingleSelectHierarchy()
        {
            var viewModel = new SingleSelectHierarchy(
                "name",
                "label",
                new ObservableCollection<ITreeNode>(),
                Mock.Of<IPromptItem>());

            var expected = new UserControl();

            _singleSelectHierarchyViewProvider.Setup(p => p.Get(viewModel)).Returns(expected);

            var actual = _provider.Get(viewModel);

            Assert.AreEqual(actual, expected);
        }

        public class MultiSelectHierarchyTestImpl : MultiSelectHierarchy
        {
            public MultiSelectHierarchyTestImpl() : base(
                "name", 
                "label", 
                new ObservableCollection<ITreeNode>(), 
                new ObservableCollection<ITreeNode>())
            {
            }
        }

        public class MultiSelectTestingImpl : MultiSelectSearch
        {
            public MultiSelectTestingImpl()
                : base("name",
                    "label",
                    Mock.Of<ISearchService>(),
                    new ObservableCollection<ISearchablePromptItem>())
            {
            }
        }

        public class MultiSelectCasscadingSearchTestImpl : MultiSelectCasscadingSearch
        {
            public MultiSelectCasscadingSearchTestImpl()
                : base(
                "label",
                "name",
                Mock.Of<IAsynchronousSearchService>(),
                new ObservableCollection<ISearchablePromptItem>(),
                new ObservableCollection<ISearchablePromptItem>())
            {
            }
        }

        public class SingleSelectTestImpl : SingleSelectPrompt<ISearchablePromptItem>
        {
            public SingleSelectTestImpl() : base(
                    "name",
                    "label",
                    new ObservableCollection<ISearchablePromptItem>(),
                    null)
            {
            }
        }

        public class EmptyPromptTestImmpl : EmptyPrompt
        {
            public EmptyPromptTestImmpl() : base("name", "label")
            {
            }
        }

        public class SingleSelectHierarchyTestImpl : SingleSelectHierarchy
        {
            public SingleSelectHierarchyTestImpl(): base(
                "name",
                "label",
                new ObservableCollection<ITreeNode>(),
                null)
            {
            }
        }
    }
}
