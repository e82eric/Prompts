using System.Linq;
using NUnit.Framework;
using Prompts.Service.PromptService;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptLevelTest
    {
        [Test]
        public void ItSetsTheAvailableItemsToAnEmptyCollectionWhenTheValidValuesAreNull()
        {
            var promptLevel = new PromptLevel("ParameterName", null, false);
            Assert.AreEqual(0, promptLevel.AvailableItems.Count());
        }
    }
}
