using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class SearchablePromptItemsTest
    {
        private Mock<ISearchablePromptItem> _fakePromptItem1;
        private Mock<ISearchablePromptItem> _fakePromptItem2;
        private Mock<ISearchablePromptItem> _fakePromptItem3;
        private SearchablePromptItems _searchablePromptValueCollection;

        [TestInitialize]
        public void Setup()
        {
            _fakePromptItem1 = new Mock<ISearchablePromptItem>();
            _fakePromptItem2 = new Mock<ISearchablePromptItem>();
            _fakePromptItem3 = new Mock<ISearchablePromptItem>();

            var fakedPromptItems = new ObservableCollection<ISearchablePromptItem>(
                A.Array(
                    _fakePromptItem1.Object,
                    _fakePromptItem2.Object,
                    _fakePromptItem3.Object));

            _searchablePromptValueCollection = new SearchablePromptItems(fakedPromptItems);
        }

        [TestMethod]
        public void ItRetunsOnlyPromptsItemsThatReturnTrueForLabelStartWithCiForLabelStartsWith()
        {
            const string searchValue = "Stub";

            _fakePromptItem1.Setup(v => v.LabelStartsWithCi(searchValue)).Returns(false);
            _fakePromptItem2.Setup(v => v.LabelStartsWithCi(searchValue)).Returns(false);
            _fakePromptItem3.Setup(v => v.LabelStartsWithCi(searchValue)).Returns(true);

            var returnedValues = _searchablePromptValueCollection.LabelStartsWith(searchValue);

            Assert.AreEqual(_fakePromptItem3.Object, returnedValues.Single());
        }

        [TestMethod]
        public void ItRetunsOnlyPromptsItemsThatReturnTrueForLabelEndsWithCiForLabelSEndsWith()
        {
            const string searchValue = "Stub";

            _fakePromptItem1.Setup(v => v.LabelEndsWithCi(searchValue)).Returns(false);
            _fakePromptItem2.Setup(v => v.LabelEndsWithCi(searchValue)).Returns(false);
            _fakePromptItem3.Setup(v => v.LabelEndsWithCi(searchValue)).Returns(true);

            var returnedValues = _searchablePromptValueCollection.LabelEndsWith(searchValue);

            Assert.AreEqual(_fakePromptItem3.Object, returnedValues.Single());
        }

        [TestMethod]
        public void ItRetunsOnlyPromptsItemsThatReturnTrueForLabelContainsCiForLabelContains()
        {
            const string searchValue = "Stub";

            _fakePromptItem1.Setup(v => v.LabelContainsCi(searchValue)).Returns(true);
            _fakePromptItem2.Setup(v => v.LabelContainsCi(searchValue)).Returns(false);
            _fakePromptItem3.Setup(v => v.LabelContainsCi(searchValue)).Returns(false);

            var returnedValues = _searchablePromptValueCollection.LabelContains(searchValue);

            Assert.AreEqual(_fakePromptItem1.Object, returnedValues.Single());
        }

        [TestMethod]
        public void ItRetunsOnlyPromptsItemsThatReturnTrueForLabelEqualsForLabelEquals()
        {
            const string searchValue = "Value 2";

            _fakePromptItem1.Setup(v => v.LabelEqualsCi(searchValue)).Returns(false);
            _fakePromptItem2.Setup(v => v.LabelEqualsCi(searchValue)).Returns(true);
            _fakePromptItem3.Setup(v => v.LabelEqualsCi(searchValue)).Returns(false);

            var returnedValues = _searchablePromptValueCollection.LabelEquals(searchValue);

            Assert.AreEqual(_fakePromptItem2.Object, returnedValues.Single());
        }
    }
}
