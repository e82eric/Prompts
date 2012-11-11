using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prompts.Prompting.ViewModels.Implementation;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class EmptyPromptTest
    {
        [TestMethod]
        public void ItIsInitalizedWithFalseForReadyForReportExeution()
        {
            var prompt = new EmptyPrompt("Name", "Label");

            Assert.IsFalse(prompt.ReadyForReportExecution);
        }

        [TestMethod]
        public void ItBecomesReadyForReportExecutionWhenTheTextIsSet()
        {
            var numberOfReportExecutionEvents = 0;
            var numberOfTextEvents = 0;

            var emptyPrompt = new EmptyPrompt("Name", "Label");
            emptyPrompt.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "ReadyForReportExecution")
                    {
                        numberOfReportExecutionEvents++;
                    }
                    if(e.PropertyName == "Text")
                    {
                        numberOfTextEvents++;
                    }
                };

            emptyPrompt.Text = "Text";

            Assert.IsTrue(emptyPrompt.ReadyForReportExecution);
            Assert.AreEqual(1, numberOfReportExecutionEvents);
            Assert.AreEqual(1, numberOfTextEvents);
        }

        [TestMethod]
        public void ItBecomesNotReadyForReportExecutionWhenTheTextIsSetToNull()
        {
            var numberOfEvents = 0;

            var emptyPrompt = new EmptyPrompt("Name", "Label");

            emptyPrompt.Text = "Text";

            emptyPrompt.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "ReadyForReportExecution")
                {
                    numberOfEvents++;
                }
            };

            emptyPrompt.Text = null;

            Assert.IsFalse(emptyPrompt.ReadyForReportExecution);
            Assert.AreEqual(1, numberOfEvents);
        }

        [TestMethod]
        public void ItBecomesNotReadyForReportExecutionWhenTheTextIsSetToBlank()
        {
            var numberOfEvents = 0;

            var emptyPrompt = new EmptyPrompt("Name", "Label");

            emptyPrompt.Text = "Text";

            emptyPrompt.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "ReadyForReportExecution")
                {
                    numberOfEvents++;
                }
            };

            emptyPrompt.Text = string.Empty;

            Assert.IsFalse(emptyPrompt.ReadyForReportExecution);
            Assert.AreEqual(1, numberOfEvents);
        }

        [TestMethod]
        public void ItUsesTheTextToCreateThePromptSelectionInfo()
        {
            var emptyPrompt = new EmptyPrompt("Name", "Label");

            emptyPrompt.Text = "Text";

            var selectionInfo = emptyPrompt.ToSelectionInfo();
            Assert.AreEqual(emptyPrompt.Text, selectionInfo.Selections.Single().Value);
            Assert.AreEqual(emptyPrompt.Name, selectionInfo.PromptName);
        }
    }
}
