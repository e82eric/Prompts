using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.Controls;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.Controls
{
    [TestClass]
    public class UniformItemsControlerTest
    {
        private Mock<IUniformItemsControl> _control;
        private UniformItemsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _control = new Mock<IUniformItemsControl>();
            _controller = new UniformItemsController(_control.Object);
        }

        [TestMethod]
        public void ItDoesNothingWhenTheItemsAreSetButTheSelectedItemIsNotSet()
        {
            var items = A.Array(A.Prompt().Build());

            _controller.Items = items;

            _control.Verify(c => c.CreateItems(items), Times.Exactly(0));
            _control.Verify(c => c.GrowItem(It.IsAny<object>(), It.IsAny<Action>()), Times.Exactly(0));
            _control.Verify(c => c.HideItem(It.IsAny<object>()), Times.Exactly(0));
        }

        [TestMethod]
        public void ItDoesNothingWhenTheSelectedItemIsSetButTheItemsAreNotSet()
        {
            _controller.SelectedItem = A.Prompt().Build();

            _control.Verify(c => c.CreateItems(It.IsAny<IEnumerable<object>>()), Times.Exactly(0));
            _control.Verify(c => c.GrowItem(It.IsAny<object>(), It.IsAny<Action>()), Times.Exactly(0));
            _control.Verify(c => c.HideItem(It.IsAny<object>()), Times.Exactly(0));
        }

        [TestMethod]
        public void ItCallsCreateOnTheControlAndShrinkOnSelectedItemAfterSelectedItemIsSetAndTheItemsAreSet()
        {
            var itemToSelect = A.Prompt().Build();
            var items = A.Array(itemToSelect);

            _controller.SelectedItem = itemToSelect;
            _controller.Items = items;

            _control.Verify(c => c.CreateItems(items), Times.Exactly(1));
            _control.Verify(c => c.GrowItem(It.IsAny<object>(), It.IsAny<Action>()), Times.Exactly(0));
            _control.Verify(c => c.HideItem(itemToSelect), Times.Exactly(1));
        }

        [TestMethod]
        public void ItCallsCreateOnTheControlAndShrinkOnSelectedItemAfterItemsAreSetAndThenSlectedItemIsSet()
        {
            var itemToSelect = A.Prompt().Build();
            var items = A.Array(itemToSelect);

            _controller.Items = items;
            _controller.SelectedItem = itemToSelect;

            _control.Verify(c => c.CreateItems(items), Times.Exactly(1));
            _control.Verify(c => c.GrowItem(It.IsAny<object>(), It.IsAny<Action>()), Times.Exactly(0));
            _control.Verify(c => c.HideItem(itemToSelect), Times.Exactly(1));
        }

        [TestMethod]
        public void ItCallsGrowOnTheOriginalSelectedItemAndShrinkOnTheNewSelectedItemWhenANewItemIsSelected()
        {
            var originalItemToSelect = A.Prompt().Build();
            var newItemToSelect = A.Prompt().Build();

            _controller.SelectedItem = originalItemToSelect;
            _controller.Items = A.Array(originalItemToSelect, newItemToSelect);

            _controller.SelectedItem = newItemToSelect;

            _control.Verify(c => c.GrowItem(originalItemToSelect, It.IsAny<Action>()), Times.Exactly(1));
            _control.Verify(c => c.HideItem(newItemToSelect), Times.Exactly(1));
        }

        [TestMethod]
        public void ItCorrectlyQuesTheHideAndGrowOperationsWhenAItemIsSelectedBeforeTheOtherCompletes()
        {
            var growCallback = new CallbackInterceptor();

            var originalItemToSelect = A.Prompt().Build();
            var itemToSelect1 = A.Prompt().Build();
            var itemToSelect2 = A.Prompt().Build();

            _controller.SelectedItem = originalItemToSelect;
            _controller.Items = A.Array(originalItemToSelect, itemToSelect1, itemToSelect2);

            _controller.SelectedItem = itemToSelect1;

            _control.Verify(c => c.GrowItem(originalItemToSelect, It.Is<Action>(a => growCallback.Intercept(a))), Times.Exactly(1));
            _control.Verify(c => c.HideItem(itemToSelect1), Times.Exactly(1));

            _controller.SelectedItem = itemToSelect2;

            _control.Verify(c => c.GrowItem(itemToSelect1, It.IsAny<Action>()), Times.Exactly(0));
            _control.Verify(c => c.HideItem(itemToSelect2), Times.Exactly(0));

            growCallback.ExecuteCallback();

            _control.Verify(c => c.GrowItem(itemToSelect1, It.IsAny<Action>()), Times.Exactly(1));
            _control.Verify(c => c.HideItem(itemToSelect2), Times.Exactly(1));
        }

        [TestMethod]
        public void ItDoesNothingWhenAnItemIsSelectedThatDoesNotExistInTheItems()
        {
            var originalSelectedItem = A.Prompt().Build();
            var items = A.Array(originalSelectedItem);

            var newItemToSelect = A.Prompt().Build();

            _controller.SelectedItem = originalSelectedItem;
            _controller.Items = items;

            _controller.SelectedItem = newItemToSelect;

            _control.Verify(c => c.HideItem(newItemToSelect), Times.Exactly(0));
            _control.Verify(c => c.GrowItem(originalSelectedItem, It.IsAny<Action>()), Times.Exactly(0));
        }

        [TestMethod]
        public void ItDoesNothingWhenTheItemsAreSetAndTheSelectedItemDoesNotExistInTheItems()
        {
            var originalSelectedItem = A.Prompt().Build();
            var items = A.Array(originalSelectedItem);

            var newItems = A.Array(A.Prompt().Build());

            _controller.SelectedItem = originalSelectedItem;
            _controller.Items = items;

            _controller.Items = newItems;

            _control.Verify(c => c.CreateItems(newItems), Times.Exactly(0));
        }

        [TestMethod]
        public void ItReCreatesTheItemsWhenAnItemIsSelectedThatDoesNotExistInTheItemsAndThenTheItemsAreSetAndTheSelectedItemsExistIn()
        {
            var originalSelectedItem = A.Prompt().Build();
            var items = A.Array(originalSelectedItem);

            var newItemToSelect = A.Prompt().Build();
            var newItems = A.Array(newItemToSelect);

            _controller.SelectedItem = originalSelectedItem;
            _controller.Items = items;

            _controller.SelectedItem = newItemToSelect;
            _controller.Items = newItems;

            _control.Verify(c => c.CreateItems(newItems), Times.Exactly(1));
            _control.Verify(c => c.HideItem(newItemToSelect), Times.Exactly(1));
        }

        [TestMethod]
        public void ItReCreatesTheItemsWhenTheItemsAreSetToACollectionThatTheSelectedDoesNotBelongToAndThenTheSelectedItemIsSetToOneFromTheNewItems()
        {
            var originalSelectedItem = A.Prompt().Build();
            var items = A.Array(originalSelectedItem);

            var newItemToSelect = A.Prompt().Build();
            var newItems = A.Array(newItemToSelect);

            _controller.SelectedItem = originalSelectedItem;
            _controller.Items = items;

            _controller.Items = newItems;
            _controller.SelectedItem = newItemToSelect;

            _control.Verify(c => c.CreateItems(newItems), Times.Exactly(1));
            _control.Verify(c => c.HideItem(newItemToSelect), Times.Exactly(1));
        }

        [TestMethod]
        public void ItIsAbleToSelectAnItemAfterItReCreatesTheItems()
        {
            var originalSelectedItem = A.Prompt().Build();
            var items = A.Array(originalSelectedItem);

            var newItemToSelect = A.Prompt().Build();
            var newItemToSelect2 = A.Prompt().Build();
            var newItems = A.Array(newItemToSelect, newItemToSelect2);

            _controller.SelectedItem = originalSelectedItem;
            _controller.Items = items;

            _controller.Items = newItems;
            _controller.SelectedItem = newItemToSelect;

            _controller.SelectedItem = newItemToSelect2;

            _control.Verify(c => c.GrowItem(newItemToSelect, It.IsAny<Action>()), Times.Exactly(1));
            _control.Verify(c => c.HideItem(newItemToSelect2), Times.Exactly(1));
        }

        [TestMethod]
        public void ItIsAbleToSelectAnItemAfterItReCreatesTheItems2()
        {
            var originalSelectedItem = A.Prompt().Build();
            var items = A.Array(originalSelectedItem);

            var newItemToSelect = A.Prompt().Build();
            var newItemToSelect2 = A.Prompt().Build();
            var newItems = A.Array(newItemToSelect, newItemToSelect2);

            _controller.SelectedItem = originalSelectedItem;
            _controller.Items = items;

            _controller.SelectedItem = newItemToSelect;
            _controller.Items = newItems;

            _controller.SelectedItem = newItemToSelect2;

            _control.Verify(c => c.GrowItem(newItemToSelect, It.IsAny<Action>()), Times.Exactly(1));
            _control.Verify(c => c.HideItem(newItemToSelect2), Times.Exactly(1));
        }

        [TestMethod]
        public void ItIsAbleToRecreateTheItemsMoreThanOnce()
        {
            var selectedItem1 = A.Prompt().Build();
            var items1 = A.Array(selectedItem1);

            var selectedItem2 = A.Prompt().Build();
            var items2 = A.Array(selectedItem2);

            var selectedItem3 = A.Prompt().Build();
            var items3 = A.Array(selectedItem3);

            _controller.SelectedItem = selectedItem1;
            _controller.Items = items1;

            _controller.SelectedItem = selectedItem2;
            _controller.Items = items2;

            _controller.SelectedItem = selectedItem3;
            _controller.Items = items3;
        }

        [TestMethod]
        public void ItIsAbleToRecreateTheItemsMoreThanOnce2()
        {
            var selectedItem1 = A.Prompt().Build();
            var items1 = A.Array(selectedItem1);

            var selectedItem2 = A.Prompt().Build();
            var items2 = A.Array(selectedItem2);

            var selectedItem3 = A.Prompt().Build();
            var items3 = A.Array(selectedItem3);

            _controller.Items = items1;
            _controller.SelectedItem = selectedItem1;

            _controller.Items = items2;
            _controller.SelectedItem = selectedItem2;

            _controller.Items = items3;
            _controller.SelectedItem = selectedItem3;
        }
    }
}
