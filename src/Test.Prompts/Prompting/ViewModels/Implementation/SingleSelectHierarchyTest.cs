using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class SingleSelectHierarchyTest
    {
        private ITreeNode _rootNode1;
        private ITreeNode _childNode1;
        private ITreeNode _childNode2;
        private ITreeNode _grandChildNode1;
        private SingleSelectHierarchy _singleSelectHierarchy;
        private ObservableCollection<ITreeNode> _availableItems;

        [TestInitialize]
        public void Setup()
        {
            _grandChildNode1 = CreateNodeWithoutChildren();
            _childNode2 = CreateNode(new[] {_grandChildNode1});
            _childNode1 = CreateNodeWithoutChildren();
            _rootNode1 = CreateNode(new[] {_childNode1, _childNode2});

            _availableItems = A.ObservableCollection(_rootNode1);

            _singleSelectHierarchy = new SingleSelectHierarchy("Name", "label", _availableItems, null);
        }

        [TestMethod]
        public void ItCanSelectAnItemAtTheTopOfTheAvailableItemHierarchy()
        {
            Assert.IsNull(_singleSelectHierarchy.SelectedItem);
            _singleSelectHierarchy.SelectedItem = _rootNode1;
            Assert.AreEqual(_rootNode1, _singleSelectHierarchy.SelectedItem);
        }

        [TestMethod]
        public void ItCanSelectAnItemAtTheSecondLevelOfTheAvailableItemHierarchy()
        {
            Assert.IsNull(_singleSelectHierarchy.SelectedItem);
            _singleSelectHierarchy.SelectedItem = _childNode1;
            Assert.AreEqual(_childNode1, _singleSelectHierarchy.SelectedItem);
        }

        [TestMethod]
        public void ItCanSelectAnItemAtTheBottomLevelOfTheAvailableItemHierarchy()
        {
            Assert.IsNull(_singleSelectHierarchy.SelectedItem);
            _singleSelectHierarchy.SelectedItem = _grandChildNode1;
            Assert.AreEqual(_grandChildNode1, _singleSelectHierarchy.SelectedItem);
        }

        [TestMethod]
        public void ItCanSetTheTopLevelAsTheDefaultSelection()
        {
            var node1 = new Mock<ITreeNode>();
            var node2 = new Mock<ITreeNode>();
            var node3 = new Mock<ITreeNode>();
            var nodes = A.ObservableCollection(node1.Object, node2.Object, node3.Object);

            var prompt = new SingleSelectHierarchy("name", "label", nodes, node2.Object);

            Assert.AreEqual(node2.Object, prompt.SelectedItem);

            node2.VerifySet(n => n.IsSelected = true, Times.Exactly(1));
        }

        [TestMethod]
        public void ItCanSelectATopNode()
        {
            var node1 = new Mock<ITreeNode>();
            var node2 = new Mock<ITreeNode>();
            var node3 = new Mock<ITreeNode>();
            var nodes = A.ObservableCollection(node1.Object, node2.Object, node3.Object);

            var prompt = new SingleSelectHierarchy("name", "label", nodes, null);

            prompt.SelectedItem = node2.Object;

            node2.VerifySet(n => n.IsSelected = true, Times.Exactly(1));
        }


        [TestMethod]
        public void ItDeselectsThePreviousSelectionWhenANewNodeIsSelected()
        {
            var node1 = new Mock<ITreeNode>();
            var node2 = new Mock<ITreeNode>();
            var node3 = new Mock<ITreeNode>();
            var nodes = A.ObservableCollection(node1.Object, node2.Object, node3.Object);

            var prompt = new SingleSelectHierarchy("name", "label", nodes, null);

            prompt.SelectedItem = node2.Object;

            prompt.SelectedItem = node1.Object;

            node2.VerifySet(n => n.IsSelected = false, Times.Exactly(1));
            node1.VerifySet(n => n.IsSelected = true, Times.Exactly(1));
        }

        [TestMethod]
        public void ItCanSelectAChildNode()
        {
            var node1 = new Mock<ITreeNode>();
            var node2 = new Mock<ITreeNode>();
            var node3 = new Mock<ITreeNode>();
            var child1 = new Mock<ITreeNode>();
            var child2 = new Mock<ITreeNode>();
            var child3 = new Mock<ITreeNode>();
            node2.SetupGet(n => n.Children).Returns(A.ObservableCollection(child1.Object, child2.Object, child3.Object));
            var nodes = A.ObservableCollection(node1.Object, node2.Object, node3.Object);

            var prompt = new SingleSelectHierarchy("name", "label", nodes, null);

            prompt.SelectedItem = child2.Object;

            child2.VerifySet(n => n.IsSelected = true, Times.Exactly(1));
        }

        [TestMethod]
        public void ItCanChangeSelectionFromChildToRootNode()
        {
            var node1 = new Mock<ITreeNode>();
            var node2 = new Mock<ITreeNode>();
            var node3 = new Mock<ITreeNode>();
            var child1 = new Mock<ITreeNode>();
            var child2 = new Mock<ITreeNode>();
            var child3 = new Mock<ITreeNode>();
            node2.SetupGet(n => n.Children).Returns(A.ObservableCollection(child1.Object, child2.Object, child3.Object));
            var nodes = A.ObservableCollection(node1.Object, node2.Object, node3.Object);

            var prompt = new SingleSelectHierarchy("name", "label", nodes, null);

            prompt.SelectedItem = child2.Object;
            prompt.SelectedItem = node2.Object;

            child2.VerifySet(n => n.IsSelected = false, Times.Exactly(1));
            node2.VerifySet(n => n.IsSelected = true, Times.Exactly(1));
        }


        [TestMethod]
        public void ItCanSelectAGrandChildNode()
        {
            var node1 = new Mock<ITreeNode>();
            var node2 = new Mock<ITreeNode>();
            var node3 = new Mock<ITreeNode>();
            var child1 = new Mock<ITreeNode>();
            var child2 = new Mock<ITreeNode>();
            var child3 = new Mock<ITreeNode>();
            var grandChild1 = new Mock<ITreeNode>();
            var grandChild2 = new Mock<ITreeNode>();
            var grandChild3 = new Mock<ITreeNode>();
            node2.SetupGet(n => n.Children).Returns(A.ObservableCollection(child1.Object, child2.Object, child3.Object));
            child2.SetupGet(n => n.Children).Returns(
                A.ObservableCollection(grandChild1.Object, grandChild2.Object, grandChild3.Object));
            var nodes = A.ObservableCollection(node1.Object, node2.Object, node3.Object);

            var prompt = new SingleSelectHierarchy("name", "label", nodes, null);

            prompt.SelectedItem = grandChild2.Object;

            grandChild2.VerifySet(n => n.IsSelected = true, Times.Exactly(1));
        }

        private static ITreeNode CreateNodeWithoutChildren()
        {
            return CreateNode(new ITreeNode[]{});
        }

        private static ITreeNode CreateNode(params ITreeNode[] children)
        {
            var mockTreeNode = new Mock<ITreeNode>();
            mockTreeNode.SetupGet(n => n.Children).Returns(A.ObservableCollection(children));
            return mockTreeNode.Object;
        }
    }
}
