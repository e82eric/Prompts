using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Controls;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Controls
{
    [TestClass]
    public class TreeItemHierarchyFlattnerTest
    {
        [TestMethod]
        public void ItReturnsOneItemWhenThereIsOnlyOneItemAndNoChildren()
        {
            var treeItem = Mock.Of<ITreeItem>();

            var collection = new TreeItemHierarchyFlattener();

            var flattenedHierarchy = collection.Flatten(A.Array(treeItem));
            flattenedHierarchy.AssertContains(treeItem);
        }

        [TestMethod]
        public void ItReturnsTheRootItemsWhenThereAreNoChildren()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = Mock.Of<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();

            var collection = new TreeItemHierarchyFlattener();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2, treeItem3));

            flattenedHierarchy.AssertContains(treeItem1, treeItem2, treeItem3);
        }

        [TestMethod]
        public void ItReturnsTheChildrenAfterTheParent()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();
            var child1 = Mock.Of<ITreeItem>();
            var child2 = Mock.Of<ITreeItem>();

            treeItem2.SetupGet(i => i.IsExpanded).Returns(true);
            treeItem2.SetupGet(i => i.Children).Returns(A.Array(child1, child2));

            var collection = new TreeItemHierarchyFlattener();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2.Object, treeItem3));
            flattenedHierarchy.AssertContains(treeItem1, treeItem2.Object, child1, child2, treeItem3);
        }

        [TestMethod]
        public void ItDoesNotIncludeTheChildrenWhenNotExpanded()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();
            var child1 = Mock.Of<ITreeItem>();
            var child2 = Mock.Of<ITreeItem>();

            treeItem2.SetupGet(i => i.IsExpanded).Returns(false);
            treeItem2.SetupGet(i => i.Children).Returns(A.Array(child1, child2));

            var collection = new TreeItemHierarchyFlattener();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2.Object, treeItem3));
            Assert.AreEqual(3, flattenedHierarchy.Count);
            flattenedHierarchy.AssertContains(treeItem1, treeItem2.Object, treeItem3);
        }

        [TestMethod]
        public void ItReturnsTheGrandChildrenChildrenAfterTheChild()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();
            var child1 = Mock.Of<ITreeItem>();
            var child2 = new Mock<ITreeItem>();
            var child3 = Mock.Of<ITreeItem>();
            var grandChild1 = Mock.Of<ITreeItem>();
            var grandChild2 = Mock.Of<ITreeItem>();

            treeItem2.SetupGet(i => i.IsExpanded).Returns(true);
            treeItem2.SetupGet(i => i.Children).Returns(A.Array(child1, child2.Object, child3));
            child2.SetupGet(i => i.IsExpanded).Returns(true);
            child2.SetupGet(i => i.Children).Returns(A.Array(grandChild1, grandChild2));

            var collection = new TreeItemHierarchyFlattener();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2.Object, treeItem3));
            flattenedHierarchy.AssertContains(
                treeItem1
                , treeItem2.Object
                , child1
                , child2.Object
                , grandChild1
                , grandChild2
                , child3
                , treeItem3);
        }
    }

    [TestClass]
    public class TreeItemHierarchyFlattner2Test
    {
        [TestMethod]
        public void ItReturnsOneItemWhenThereIsOnlyOneItemAndNoChildren()
        {
            var treeItem = Mock.Of<ITreeItem>();

            var collection = new TreeItemHierarchyFlattener2();

            var flattenedHierarchy = collection.Flatten(A.Array(treeItem));
            flattenedHierarchy.AssertContains(treeItem);
        }

        [TestMethod]
        public void ItReturnsTheRootItemsWhenThereAreNoChildren()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = Mock.Of<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();

            var collection = new TreeItemHierarchyFlattener2();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2, treeItem3));

            flattenedHierarchy.AssertContains(treeItem1, treeItem2, treeItem3);
        }

        [TestMethod]
        public void ItReturnsTheChildrenAfterTheParent()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();
            var child1 = Mock.Of<ITreeItem>();
            var child2 = Mock.Of<ITreeItem>();

            treeItem2.SetupGet(i => i.IsExpanded).Returns(true);
            treeItem2.SetupGet(i => i.Children).Returns(A.Array(child1, child2));

            var collection = new TreeItemHierarchyFlattener2();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2.Object, treeItem3));
            flattenedHierarchy.AssertContains(treeItem1, treeItem2.Object, child1, child2, treeItem3);
        }

        [TestMethod]
        public void ItDoesIncludeTheChildrenWhenNotExpanded()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();
            var child1 = Mock.Of<ITreeItem>();
            var child2 = Mock.Of<ITreeItem>();

            treeItem2.SetupGet(i => i.IsExpanded).Returns(false);
            treeItem2.SetupGet(i => i.Children).Returns(A.Array(child1, child2));

            var collection = new TreeItemHierarchyFlattener2();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2.Object, treeItem3));
            flattenedHierarchy.AssertContains(treeItem1, treeItem2.Object, treeItem3, child1, child2);
        }

        [TestMethod]
        public void ItReturnsTheGrandChildrenChildrenAfterTheChild()
        {
            var treeItem1 = Mock.Of<ITreeItem>();
            var treeItem2 = new Mock<ITreeItem>();
            var treeItem3 = Mock.Of<ITreeItem>();
            var child1 = Mock.Of<ITreeItem>();
            var child2 = new Mock<ITreeItem>();
            var child3 = Mock.Of<ITreeItem>();
            var grandChild1 = Mock.Of<ITreeItem>();
            var grandChild2 = Mock.Of<ITreeItem>();

            treeItem2.SetupGet(i => i.IsExpanded).Returns(true);
            treeItem2.SetupGet(i => i.Children).Returns(A.Array(child1, child2.Object, child3));
            child2.SetupGet(i => i.IsExpanded).Returns(true);
            child2.SetupGet(i => i.Children).Returns(A.Array(grandChild1, grandChild2));

            var collection = new TreeItemHierarchyFlattener2();
            var flattenedHierarchy = collection.Flatten(A.Array(treeItem1, treeItem2.Object, treeItem3));
            flattenedHierarchy.AssertContains(
                treeItem1
                , treeItem2.Object
                , child1
                , child2.Object
                , grandChild1
                , grandChild2
                , child3
                , treeItem3);
        }
    }
}
