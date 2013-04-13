using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.MainPage;

namespace Test.Prompts
{
    [TestClass]
    public class MainPageViewModelTest
    {
        private MainPageViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new MainPageViewModel();
        }

        [TestMethod]
        public void ItInitalizesTheViewModelWithShowForTheShowHideText()
        {
            Assert.IsFalse(_viewModel.IsCollapsed);
            Assert.IsTrue(_viewModel.ShowHideCommand.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheShowHideTextToShowWhenTheShowHideCommandIsExecuted()
        {
            var numberOfEvents = 0;

            _viewModel.PropertyChanged += (s, e) =>
                {
                    if(e.PropertyName == "IsCollapsed")
                    {
                        numberOfEvents++;
                    }
                };

            _viewModel.ShowHideCommand.Execute(null);

            Assert.AreEqual(1, numberOfEvents);
            Assert.IsTrue(_viewModel.IsCollapsed);
            Assert.IsTrue(_viewModel.ShowHideCommand.CanExecute(null));
        }

        [TestMethod]
        public void ItSetsTheShowHideTextToBackToHideWhenItIsExecutedAgain()
        {
            var numberOfEvents = 0;

            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "IsCollapsed")
                {
                    numberOfEvents++;
                }
            };

            _viewModel.ShowHideCommand.Execute(null);

            Assert.AreEqual(1, numberOfEvents);
            Assert.IsTrue(_viewModel.ShowHideCommand.CanExecute(null));

            _viewModel.ShowHideCommand.Execute(null);

            Assert.AreEqual(2, numberOfEvents);

            Assert.IsFalse(_viewModel.IsCollapsed);
            Assert.IsTrue(_viewModel.ShowHideCommand.CanExecute(null));
        }
    }
}
