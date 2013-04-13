using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.PromptService;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class EmptyPromptBuilderTest
    {
        [TestMethod]
        public void ItSetsTheTestToTheDefaultValueIfThereIsOne()
        {
            var builder = new EmptyPromptBuilder();

            var promptInfo = A.PromptInfo()
                .WithDefaultValues(A.ObservableCollection(A.DefaultValue().WithValue("Value").Build()))
                .Build();

            var prompt = (EmptyPrompt)builder.BuildFrom(promptInfo);
            Assert.AreEqual(promptInfo.DefaultValues.Single().Value, prompt.Text);
        }

        [TestMethod]
        public void ItDoesNotSetTheTextIfTheDefaultsAreEmpty()
        {
            var builder = new EmptyPromptBuilder();

            var promptInfo = A.PromptInfo()
                .WithDefaultValues(new ObservableCollection<DefaultValue>())
                .Build();

            var prompt = (EmptyPrompt)builder.BuildFrom(promptInfo);
            Assert.AreEqual(null, prompt.Text);
        }

        [TestMethod]
        public void ItDoesNotSetTheTextIfTheDefaultsAreNull()
        {
            var builder = new EmptyPromptBuilder();

            var promptInfo = A.PromptInfo()
                .WithDefaultValues(null)
                .Build();

            var prompt = (EmptyPrompt)builder.BuildFrom(promptInfo);
            Assert.AreEqual(null, prompt.Text);
        }
    }
}
