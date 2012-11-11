using System.Windows.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Controls;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Controls
{
    [TestClass]
    public class MultiSelectorTest
    {
        private Mock<IRangeSelector> _rangeSelector;
        private MultiSelector _selectionService;
        private Mock<ISelector> _inverseSelector;
        private Mock<ISelector> _regularSelector;
        private Mock<ITreeItemHierarchyFlattener> _flattener;

        [TestInitialize]
        public void Setup()
        {
            _flattener = new Mock<ITreeItemHierarchyFlattener>();
            _rangeSelector = new Mock<IRangeSelector>();
            _inverseSelector = new Mock<ISelector>();
            _regularSelector = new Mock<ISelector>();
            _selectionService = new MultiSelector(_rangeSelector.Object
                , _inverseSelector.Object
                , _regularSelector.Object);
        }

        [TestMethod]
        public void ItUsesTheRangeSelectorWhenTheModifierIsTheShiftKey()
        {
            var item1 = Mock.Of<ITreeItem>();
            var item2 = Mock.Of<ITreeItem>();
            var item3 = Mock.Of<ITreeItem>();
            var items = A.Array(item1, item2, item3);

            _selectionService.Select(ModifierKeys.Shift, items, item1, item2);
            _rangeSelector.Verify(s => s.Select(items, item1, item2));
        }

        [TestMethod]
        public void ItDoesNotChangeTheOldItemWhenAnotherShiftSelection()
        {
            var item1 = Mock.Of<ITreeItem>();
            var item2 = Mock.Of<ITreeItem>();
            var item3 = Mock.Of<ITreeItem>();
            var item4 = Mock.Of<ITreeItem>();
            var item5 = Mock.Of<ITreeItem>();
            var items = A.Array(item1, item2, item3, item4, item5);

            _selectionService.Select(ModifierKeys.Shift, items, item1, item2);
            _rangeSelector.Verify(s => s.Select(items, item1, item2));

            _selectionService.Select(ModifierKeys.Shift, items, item3, item1);
            _rangeSelector.Verify(s => s.Select(items, item3, item2));

            _selectionService.Select(ModifierKeys.Shift, items, item4, item3);
            _rangeSelector.Verify(s => s.Select(items, item4, item2));
        }

        [TestMethod]
        public void ItDoesNotChangeTheOldItemWhenAnotherShiftSelection3()
        {
            var item1 = Mock.Of<ITreeItem>();
            var item2 = Mock.Of<ITreeItem>();
            var item3 = Mock.Of<ITreeItem>();
            var item4 = Mock.Of<ITreeItem>();
            var item5 = Mock.Of<ITreeItem>();
            var items = A.Array(item1, item2, item3, item4, item5);

            _selectionService.Select(ModifierKeys.Shift, items, item1, item2);
            _rangeSelector.Verify(s => s.Select(items, item1, item2));

            _selectionService.Select(ModifierKeys.Shift, items, item3, item1);
            _rangeSelector.Verify(s => s.Select(items, item3, item2));

            _selectionService.Select(ModifierKeys.None, items, item4, item3);

            _selectionService.Select(ModifierKeys.Shift, items, item2, item4);
            _rangeSelector.Verify(s => s.Select(items, item2, item4));
        }

        [TestMethod]
        public void ItDoesNotChangeTheOldItemWhenAnotherShiftSelection2()
        {
            var item1 = Mock.Of<ITreeItem>();
            var item2 = Mock.Of<ITreeItem>();
            var item3 = Mock.Of<ITreeItem>();
            var item4 = Mock.Of<ITreeItem>();
            var item5 = Mock.Of<ITreeItem>();
            var items = A.Array(item1, item2, item3, item4, item5);

            _selectionService.Select(ModifierKeys.Shift, items, item1, item2);
            _rangeSelector.Verify(s => s.Select(items, item1, item2));

            _selectionService.Select(ModifierKeys.Shift, items, item3, item1);
            _rangeSelector.Verify(s => s.Select(items, item3, item2));

            _selectionService.Select(ModifierKeys.Control, items, item4, item3);

            _selectionService.Select(ModifierKeys.Shift, items, item2, item4);
            _rangeSelector.Verify(s => s.Select(items, item2, item4));
        }

        [TestMethod]
        public void ItUsesTheRegularSelectorWhenTheNeitherTheShiftKeyOrControlKeyIsPressed()
        {
            var item1 = Mock.Of<ITreeItem>();
            var item2 = Mock.Of<ITreeItem>();
            var item3 = Mock.Of<ITreeItem>();
            var items = A.Array(item1, item2, item3);

            _selectionService.Select(ModifierKeys.None, items, item1, item2);
            _regularSelector.Verify(s => s.Select(items, item1), Times.Exactly(1));
        }

        [TestMethod]
        public void ItUsesTheInverseSelectorWhenTheControlKeyIsPressed()
        {
            var item1 = Mock.Of<ITreeItem>();
            var item2 = Mock.Of<ITreeItem>();
            var item3 = Mock.Of<ITreeItem>();
            var items = A.Array(item1, item2, item3);

            _selectionService.Select(ModifierKeys.Control, items, item1, item2);
            _inverseSelector.Verify(s => s.Select(items, item1), Times.Exactly(1));
        }
    }
}
