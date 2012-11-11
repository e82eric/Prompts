using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Controls;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Controls
{
    [TestClass]
    public class SelectorTest
    {
        [TestMethod]
        public void ItSelectsTheItemAndDeSelectsAllOfTheOthers()
        {
            var item1 = new Mock<ITreeItem>();
            var item2 = new Mock<ITreeItem>();
            var item3 = new Mock<ITreeItem>();
            var item4 = new Mock<ITreeItem>();
            var rootItems = new List<ITreeItem> {item1.Object, item2.Object};
            var items = A.Array(item1.Object, item2.Object, item3.Object, item4.Object);

            var flattener = Mock.Of<ITreeItemHierarchyFlattener>(f => f.Flatten(rootItems) == items);
            var selector = new Selector(flattener);
            
            selector.Select(rootItems, item3.Object);

            item1.VerifySet(i => i.IsSelected2 = false, Times.Exactly(1));
            item2.VerifySet(i => i.IsSelected2 = false, Times.Exactly(1));
            item3.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
            item4.VerifySet(i => i.IsSelected2 = false, Times.Exactly(1));
        }
    }
}
