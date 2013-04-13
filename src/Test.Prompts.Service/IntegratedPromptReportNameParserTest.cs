using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class IntegratedPromptReportNameParserTest
    {
        [Test]
        public void ItAddDotRDLToThePromptName()
        {
            const string promptName = "PromptName";

            var promptReportNameParser = new IntegratedPromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(string.Format("{0}.rdl", promptName), promptReportName);
        }

        [Test]
        public void ItReturnsEverythingBeforeTheUnderscoreWhenThePromptNameContainsAnUnderscoreWhenThePromptNameStartsWithAUnderscore()
        {

            const string promptReportname = "PromptName";
            var expectedPromptReportName = string.Format("{0}.rdl", promptReportname);

            var promptName = string.Format("A_{0}_Alias", promptReportname);

            var promptReportNameParser = new IntegratedPromptReportNameParser();

            var actualPromptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(expectedPromptReportName, actualPromptReportName);
        }

        [Test]
        public void ItReturnsThePromptNameWhenThePromptNameIsNotPrefixedWithUnderscoreA()
        {
            const string promptName = "PromptName_Alias";

            var promptReportNameParser = new IntegratedPromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(string.Format("{0}.rdl", promptName), promptReportName);
        }

        [Test]
        public void ItRemovesTheAUnderscorePrefixAndReturnsEverythingBeforeTheFinalUnderscoreWhenThereAreTwoUnderscores()
        {
            const string promptReportName = "Prompt_Name";
            var expectedPromptReportName = string.Format("{0}.rdl", promptReportName);
            var promptName = string.Format("A_{0}_Alias", promptReportName);

            var promptReportNameParser = new IntegratedPromptReportNameParser();

            var actualPromptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(expectedPromptReportName, actualPromptReportName);
        }

        [Test]
        public void ItRemovesTheAUnderscorePrefixAndReturnsEverythingBeforeTheFinalUnderscoreWhenThereAreThreeUnderscores()
        {
            const string promptReportName = "Prompt_Name_Test";
            var expectedPromptReportName = string.Format("{0}.rdl", promptReportName);

            var promptName = string.Format("A_{0}_Alias", promptReportName);

            var promptReportNameParser = new IntegratedPromptReportNameParser();

            var actualPromptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(expectedPromptReportName, actualPromptReportName);
        }

    }
}
