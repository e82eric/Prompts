using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class SingleSelectPromptTest
    {
        private SingleSelectPrompt<IPromptItem> _collection;
        private IPromptItem _promptItem1;
        private IPromptItem _promptItem2;
        private IPromptItem _promptItem3;
        private ObservableCollection<IPromptItem> _promptItems;

        [TestInitialize]
        public void Setup()
        {
            Mock.Of<IPromptItem>(i => i.Value == "Value 1");
            _promptItem1 = Mock.Of<IPromptItem>(i => i.Value == "Value 1");
            _promptItem2 = Mock.Of<IPromptItem>(i => i.Value == "Value 1");
            _promptItem3 = Mock.Of<IPromptItem>(i => i.Value == "Value 1");
            _promptItems = A.ObservableCollection(_promptItem1, _promptItem2, _promptItem3);
        }

        [TestMethod]
        public void SelectedItemCanBeSetThroughTheConstructor()
        {
            _collection = new SingleSelectPrompt<IPromptItem>("Name", "Label", _promptItems, _promptItem1);

            Assert.AreEqual(_collection.SelectedItem, _promptItem1);
        }

        [TestMethod]
        public void ReadyForReportExecutionIsTrueWhenSelectedItemIsPassedThroughTheConstructor()
        {
            _collection = new SingleSelectPrompt<IPromptItem>("Name", "Label", _promptItems, _promptItem1);

            Assert.IsTrue(_collection.ReadyForReportExecution);
        }

        [TestMethod]
        public void ANullCanBePassedIntoTheConstructorForSelectedItem()
        {
            _collection = new SingleSelectPrompt<IPromptItem>("Name", "Label", _promptItems, null);

            Assert.IsNull(_collection.SelectedItem);
        }

        [TestMethod]
        public void ReadyForExecutionChangesToTrueAndAnEventIsRaisedWhenAnItemIsSelected()
        {
            var numberOfEvents = 0;

            _collection = new SingleSelectPrompt<IPromptItem>("Name", "Label", _promptItems, null);

            _collection.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfEvents++;
                    Assert.IsTrue(_collection.ReadyForReportExecution);
                };

            _collection.SelectedItem = _promptItem1;

            Assert.AreEqual(1, numberOfEvents);
        }

        [TestMethod]
        public void ReadyStaysTrueAndNoEventIsRaisedWhenSelectedItemChangesFromOneItemToAnother()
        {
            var numberOfEvents = 0;

            _collection = new SingleSelectPrompt<IPromptItem>("Name", "Label", _promptItems, null)
                {
                    SelectedItem = _promptItem1
                };

            _collection.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfEvents++;
                    Assert.IsTrue(_collection.ReadyForReportExecution);
                };

            _collection.SelectedItem = _promptItem2;

            Assert.AreEqual(0, numberOfEvents);
        }

        [TestMethod]
        public void ReadyForExecutionChangesToFalseAndAnEventIsRaisedWhenTheSelectionItemChangesFromAnItemToNull()
        {
            var numberOfCalls = 0;

            _collection = new SingleSelectPrompt<IPromptItem>("Name", "Label", _promptItems, null)
                {
                    SelectedItem = _promptItem1
                };

            _collection.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfCalls++;
                    Assert.IsFalse(_collection.ReadyForReportExecution);
                };

            _collection.SelectedItem = null;

            Assert.AreEqual(1, numberOfCalls);
        }

        [TestMethod]
        public void ItSetsTheAvailableItemsDuringConstruction()
        {
            var expected = A.ObservableCollection(new Mock<IPromptItem>().Object);

            var dropDown = new SingleSelectPrompt<IPromptItem>("Name", "Label", expected, null);

            Assert.AreEqual(expected, dropDown.AvailableItems);
        }

        [TestMethod]
        public void RaisesEventWhenItemIsSelected()
        {
            var numberOfSelectedItemEvents = 0;

            var itemToSelect = new Mock<IPromptItem>();
            itemToSelect.SetupGet(i => i.Value).Returns("Value");

            var availableItems = A.ObservableCollection(itemToSelect.Object);

            var dropDown = new SingleSelectPrompt<IPromptItem>("Name", "Label", availableItems, null);

            dropDown.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "SelectedItem")
                {
                    numberOfSelectedItemEvents++;
                }
            };

            Assert.AreEqual(0, numberOfSelectedItemEvents);

            dropDown.SelectedItem = itemToSelect.Object;

            Assert.AreEqual(1, numberOfSelectedItemEvents);
        }
    }
}