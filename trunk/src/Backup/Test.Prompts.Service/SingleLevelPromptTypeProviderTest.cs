using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class SingleLevelPromptTypeProviderTest
    {
        private SingleLevelPromptTypeProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new SingleLevelPromptTypeProvider();
        }

        [Test]
        public void ItReturnsDropDownWhenSingleSelect()
        {
            const SelectionType selectionType = SelectionType.SingleSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.DropDown, promptType);
        }

        [Test]
        public void ItReturnsShoppingCartWhenSingleSelect()
        {
            const SelectionType selectionType = SelectionType.MultiSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.ShoppingCart, promptType);
        }
    }
}
