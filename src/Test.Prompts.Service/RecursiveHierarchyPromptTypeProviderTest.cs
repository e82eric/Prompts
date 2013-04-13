using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class RecursiveHierarchyPromptTypeProviderTest
    {
        private RecursiveHierarchyPromptTypeProvider _provider;

        [SetUp]
        public void Setup()
        {
            _provider = new RecursiveHierarchyPromptTypeProvider();
        }

        [Test]
        public void ItReturnsSingleSelectTreeWhenSingleSelect()
        {
            const SelectionType selectionType = SelectionType.SingleSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.RecursiveSingleSelectTree, promptType);
        }

        [Test]
        public void ItReturnsTreeWhenMultiSelect()
        {
            const SelectionType selectionType = SelectionType.MultiSelect;

            var promptType = _provider.GetPromptType(selectionType);

            Assert.AreEqual(PromptType.RecursiveTree, promptType);
        }
    }
}