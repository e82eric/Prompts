using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class HierarchyPromptTypeProviderTest
    {
        private HierarchyPromptTypeProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new HierarchyPromptTypeProvider();
        }

        [Test]
        public void ItReturnsSingleSelectTreeWhenSingleSelect()
        {
            const SelectionType selectionType = SelectionType.SingleSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.SingleSelectTree, promptType);
        }

        [Test]
        public void ItReturnsTreeWhenMultiSelect()
        {
            const SelectionType selectionType = SelectionType.MultiSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.Tree, promptType);
        }
    }
}
