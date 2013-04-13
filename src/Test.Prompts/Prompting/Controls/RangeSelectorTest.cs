using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Controls;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Controls
{
    [TestClass]
    public class RangeSelectorTest
    {
        private RangeSelector _selector;
        private Mock<ITreeItemHierarchyFlattener> _flattner;

        [TestInitialize]
        public void Setup()
        {
            _flattner = new Mock<ITreeItemHierarchyFlattener>();
            _selector = new RangeSelector(_flattner.Object);
        }

        [TestMethod]
        public void ItSelectsAllItemsBetweenTheFirstAndSecondItem()
        {
            var treeItem1 = new Mock<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = new Mock<ITreeItem>();
            var treeItem4 = new Mock<ITreeItem>();
            var rootItems = new List<ITreeItem> {treeItem1.Object, treeItem2.Object};
            var items = A.Array(treeItem1.Object, treeItem2.Object, treeItem3.Object, treeItem4.Object);

            _flattner.Setup(f => f.Flatten(rootItems)).Returns(items);

            _selector.Select(rootItems, treeItem2.Object, treeItem4.Object);
            treeItem1.VerifySet(i => i.IsSelected2 = false, Times.Exactly(1));
            treeItem2.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
            treeItem3.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
            treeItem4.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
        }

        [TestMethod]
        public void ItSelectsAllItemsBetweenTheFirstAndSecondItemInReverse()
        {
            var treeItem1 = new Mock<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = new Mock<ITreeItem>();
            var treeItem4 = new Mock<ITreeItem>();
            var rootItems = new List<ITreeItem> { treeItem1.Object, treeItem2.Object };
            var items = A.Array(treeItem1.Object, treeItem2.Object, treeItem3.Object, treeItem4.Object);

            _flattner.Setup(f => f.Flatten(rootItems)).Returns(items);

            _selector.Select(rootItems, treeItem4.Object, treeItem2.Object);
            treeItem1.VerifySet(i => i.IsSelected2 = false, Times.Exactly(1));
            treeItem2.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
            treeItem3.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
            treeItem4.VerifySet(i => i.IsSelected2 = true, Times.Exactly(1));
        }
    }
}
