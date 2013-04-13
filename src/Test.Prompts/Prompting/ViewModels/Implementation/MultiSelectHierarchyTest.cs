using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class MultiSelectHierarchyTest
    {
        [TestMethod]
        public void ItSetsTheAvailableItemsDuringConstruction()
        {
            var expected = A.ObservableCollection(Mock.Of<ITreeNode>());
            var selectedItems = A.ObservableCollection(Mock.Of<ITreeNode>());

            var tree = new MultiSelectHierarchy("name", "label", expected, selectedItems);

            Assert.AreEqual(expected, tree.AvailableItems);
        }

        [TestMethod]
        public void ItUsesTheIsSelectedItemsFlagToDetermineTheSelctedAvailableItems()
        {
            var item1 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var item2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var item4 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var items = A.ObservableCollection(item1, item2, item3, item4);

            var tree = new MultiSelectHierarchy("name", "label", items, new ObservableCollection<ITreeNode>());

            tree.SelectedAvailableItems.AssertContains(item2, item4);
        }

        [TestMethod]
        public void ItIncludeSelectedChildrenInTheSlectedAvailableItems()
        {
            var child1 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var child2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var child3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var child4 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item1 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == false &&
                i.Children == A.ObservableCollection(child1, child2));
            var item2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var item4 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == true &&
                i.Children == A.ObservableCollection(child3, child4));
            var items = A.ObservableCollection(item1, item2, item3, item4);

            var tree = new MultiSelectHierarchy("name", "label", items, new ObservableCollection<ITreeNode>());

            tree.SelectedAvailableItems.AssertContains(item2, item4, child2, child4);
        }

        [TestMethod]
        public void ItIncludeSelectedGrandChildrenInTheSlectedAvailableItems()
        {
            var grandChild1 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var grandChild2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var grandChild3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var grandChild4 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var child1 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == false &&
                i.Children == A.ObservableCollection(grandChild1, grandChild2));
            var child2 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == true &&
                i.Children == A.ObservableCollection(grandChild3, grandChild4));
            var child3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var child4 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item1 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == false &&
                i.Children == A.ObservableCollection(child1, child2));
            var item2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var item4 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == true &&
                i.Children == A.ObservableCollection(child3, child4));
            var items = A.ObservableCollection(item1, item2, item3, item4);

            var tree = new MultiSelectHierarchy("name", "label", items, new ObservableCollection<ITreeNode>());

            tree.SelectedAvailableItems.AssertContains(item2, item4, child2, child4, grandChild2, grandChild4);
        }

        [TestMethod]
        public void ItDoesNotReSelectTheItemIfItHasAlreadyBeenSelected()
        {
            var grandChild1 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var grandChild2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var grandChild3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var grandChild4 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var child1 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == false &&
                i.Children == A.ObservableCollection(grandChild1, grandChild2));
            var child2 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == true &&
                i.Children == A.ObservableCollection(grandChild3, grandChild4));
            var child3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var child4 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item1 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == false &&
                i.Children == A.ObservableCollection(child1, child2));
            var item2 = Mock.Of<ITreeNode>(i => i.IsSelected == true);
            var item3 = Mock.Of<ITreeNode>(i => i.IsSelected == false);
            var item4 = Mock.Of<ITreeNode>(
                i =>
                i.IsSelected == true &&
                i.Children == A.ObservableCollection(child3, child4));
            var items = A.ObservableCollection(item1, item2, item3, item4);

            var tree = new MultiSelectHierarchy("name", "label", items, new ObservableCollection<ITreeNode>());
            tree.SelectItems.Execute(null);

            tree.SelectedItems.AssertContains(item2, item4, child2, child4, grandChild2, grandChild4);

            tree.SelectedAvailableItems.AssertContains(item2, item4, child2, child4, grandChild2, grandChild4);
        }
    }
}
