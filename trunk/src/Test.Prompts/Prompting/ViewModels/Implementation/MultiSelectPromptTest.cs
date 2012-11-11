using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class MultiSelectPromptTest
    {
        [TestMethod]
        public void ItRaisesAnEventWhenTheAvailableItemsAreSet()
        {
            var numberOfEvents = 0;

            var newAvailableItems = A.ObservableCollection(new Mock<ISearchablePromptItem>().Object);

            var shoppingCart
                = new MultiSelectPrompt<ISearchablePromptItem>(
                    "label", 
                    "name", 
                    new ObservableCollection<ISearchablePromptItem>());

            shoppingCart.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "AvailableItems")
                {
                    numberOfEvents++;
                    Assert.AreEqual(newAvailableItems, shoppingCart.AvailableItems);
                }
            };

            Assert.AreEqual(0, numberOfEvents);

            shoppingCart.AvailableItems = newAvailableItems;

            Assert.AreEqual(1, numberOfEvents);
        }

        [TestMethod]
        public void SelectedItemsCanBePassedThroughTheConstrcutor()
        {
            var selectedItems = A.ObservableCollection(Mock.Of<IPromptItem>(), Mock.Of<IPromptItem>());

            var shoppingCart = new MultiSelectPrompt<IPromptItem>("Name", "Label", selectedItems);

            Assert.AreEqual(selectedItems, shoppingCart.SelectedItems);
            Assert.IsTrue(shoppingCart.ReadyForReportExecution);
        }

        [TestMethod]
        public void SelectionsChange0To1()
        {
            var item1 = Mock.Of<IPromptItem>();

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1) };

            shoppingCart.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfEvents++;
                    Assert.IsTrue(shoppingCart.ReadyForReportExecution);
                };
            shoppingCart.SelectItems.Execute(null);
            Assert.AreEqual(1, numberOfEvents);
            shoppingCart.SelectedItems.AssertContains(item1);
        }

        [TestMethod]
        public void SelectionsChange0To3()
        {
            var item1 = Mock.Of<IPromptItem>();
            var item2 = Mock.Of<IPromptItem>();
            var item3 = Mock.Of<IPromptItem>();

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1, item2, item3) };

            shoppingCart.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfEvents++;
                    Assert.IsTrue(shoppingCart.ReadyForReportExecution);
                };
            shoppingCart.SelectItems.Execute(null);
            Assert.AreEqual(1, numberOfEvents);
            shoppingCart.SelectedItems.AssertContains(item1, item2, item3);
        }

        [TestMethod]
        public void SelectionsChangeFromOneItemToTwo()
        {
            var numberOfEvents = 0;

            var item1 = Mock.Of<IPromptItem>();
            var item2 = Mock.Of<IPromptItem>();

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1) };

            shoppingCart.SelectItems.Execute(null);

            shoppingCart.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfEvents++;
                    Assert.IsTrue(shoppingCart.ReadyForReportExecution);
                };

            shoppingCart.SelectedAvailableItems = A.Array(item2);

            shoppingCart.SelectItems.Execute(null);

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertContains(item1, item2);
        }

        [TestMethod]
        public void SameItemSelectedTwice()
        {
            var numberOfEvents = 0;

            var item1 = Mock.Of<IPromptItem>();

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1) };

            shoppingCart.SelectItems.Execute(null);

            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            shoppingCart.SelectItems.Execute(null);

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertContains(item1);
        }


        [TestMethod]
        public void SelectNullItem()
        {
            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt();

            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            shoppingCart.SelectedSelectedItems = null;
            shoppingCart.SelectItems.Execute(null);

            Assert.AreEqual(0, numberOfEvents);
        }

        [TestMethod]
        public void DeSelectNullItem()
        {
            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt();

            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            shoppingCart.SelectedSelectedItems = null;
            shoppingCart.DeSelectItems.Execute(null);

            Assert.AreEqual(0, numberOfEvents);
        }

        [TestMethod]
        public void SelectionsChangeFrom3To2()
        {
            var item1 = Mock.Of<IPromptItem>();
            var item2 = Mock.Of<IPromptItem>();
            var item3 = Mock.Of<IPromptItem>();

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1, item2, item3) };
            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            shoppingCart.SelectedSelectedItems = A.Array(item2);
            shoppingCart.DeSelectItems.Execute(null);

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertContains(item1, item3);
        }

        [TestMethod]
        public void SelectionsChangeFrom3To0()
        {
            var item1 = Mock.Of<IPromptItem>();
            var item2 = Mock.Of<IPromptItem>();
            var item3 = Mock.Of<IPromptItem>();

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1, item2, item3) };
            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) =>
                {
                    numberOfEvents++;
                    Assert.IsFalse(shoppingCart.ReadyForReportExecution);
                };

            shoppingCart.SelectedSelectedItems = A.Array(item1, item2, item3);
            shoppingCart.DeSelectItems.Execute(null);

            Assert.AreEqual(1, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(0);
        }

        [TestMethod]
        public void SelectionsChangeFrom1To0()
        {
            var item1 = Mock.Of<IPromptItem>();

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt { SelectedAvailableItems = A.Array(item1) };
            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) =>
            {
                numberOfEvents++;
                Assert.IsFalse(shoppingCart.ReadyForReportExecution);
            };

            shoppingCart.SelectedSelectedItems = A.Array(item1);
            shoppingCart.DeSelectItems.Execute(null);

            Assert.AreEqual(1, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(0);
        }

        [TestMethod]
        public void ItDeSelectsThePromptItemIfItIsMarkedAsADefaultAllAndAnotherItemIsSelected()
        {
            var item1 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == true);
            var item2 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);
            var item3 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt
                {
                    SelectedAvailableItems = A.Array(item2), 
                    SelectedItems = A.ObservableCollection(item1)
                };

            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(1);
            shoppingCart.SelectedItems.AssertContains(item2);
        }

        [TestMethod]
        public void ItDeSelectsThePromptItemIfItIsMarkedAsADefaultAllAndOtherItemsAreSelected()
        {
            var item1 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == true);
            var item2 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);
            var item3 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt
            {
                SelectedAvailableItems = A.Array(item2, item3),
                SelectedItems = A.ObservableCollection(item1)
            };

            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(2);
            shoppingCart.SelectedItems.AssertContains(item2, item3);
        }

        [TestMethod]
        public void ItDoesNotDeSelectTheDefaultAllMemberWhenTheSelectedIsExecutedWithNoSelectedAvailableItems()
        {
            var item1 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == true);
            var item2 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);
            var item3 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt
            {
                SelectedAvailableItems = null,
                SelectedItems = A.ObservableCollection(item1)
            };

            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(1);
            shoppingCart.SelectedItems.AssertContains(item1);
        }

        [TestMethod]
        public void ItDoesNotDeSelectWhenItIsMarkedDefaultAllAndIsTheSelectedAvailableItem()
        {
            var item1 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == true);
            var item2 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);
            var item3 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt
            {
                SelectedAvailableItems = A.Array(item1),
                SelectedItems = A.ObservableCollection(item1)
            };

            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(1);
            shoppingCart.SelectedItems.AssertContains(item1);
        }

        [TestMethod]
        public void ItDeselectsAllOfTheDefaultAllMembersWhenASelectIsExeduted()
        {
            var item1 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == false);
            var item2 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == true);
            var item3 = Mock.Of<IPromptItem>(i => i.IsDefaultAll == true);

            var numberOfEvents = 0;

            var shoppingCart = new TestMultiSelectPrompt
            {
                SelectedAvailableItems = A.Array(item1),
                SelectedItems = A.ObservableCollection(item2, item3)
            };

            shoppingCart.SelectItems.Execute(null);
            shoppingCart.ReadyForReportExecutionChanged += (s, e) => { numberOfEvents++; };

            Assert.AreEqual(0, numberOfEvents);
            shoppingCart.SelectedItems.AssertLength(1);
            shoppingCart.SelectedItems.AssertContains(item1);
        }

        private class TestMultiSelectPrompt : MultiSelectPrompt<IPromptItem>
        {
            public TestMultiSelectPrompt()
                : base("label", "name", new ObservableCollection<IPromptItem>())
            {
            }
        }
    }
}