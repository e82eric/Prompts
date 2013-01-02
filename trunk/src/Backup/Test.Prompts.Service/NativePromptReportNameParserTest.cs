using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class NativePromptReportNameParserTest
    {
        [Test]
        public void ItReturnsThePromptNameWhenItDoesNotContainAnUnderscore()
        {
            const string promptName = "PromptName";

            var promptReportNameParser = new NativePromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);
            
            Assert.AreEqual(promptName, promptReportName);
        }

        [Test]
        public void ItReturnsEverythingBeforeTheUnderscoreWhenThePromptNameContainsAnUnderscoreWhenThePromptNameStartsWithAUnderscore()
        {
            const string expectedPromptReportName = "PromptName";

            var promptName = string.Format("A_{0}_Alias", expectedPromptReportName);

            var promptReportNameParser = new NativePromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(expectedPromptReportName, promptReportName);
        }

        [Test]
        public void ItReturnsThePromptNameWhenThePromptNameIsNotPrefixedWithUnderscoreA()
        {
            const string promptName = "PromptName_Alias";

            var promptReportNameParser = new NativePromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(promptName, promptReportName);
        }

        [Test]
        public void ItRemovesTheAUnderscorePrefixAndReturnsEverythingBeforeTheFinalUnderscoreWhenThereAreTwoUnderscores()
        {
            const string expectedPromptReportName = "Prompt_Name";
            var promptName = string.Format("A_{0}_Alias", expectedPromptReportName);

            var promptReportNameParser = new NativePromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(expectedPromptReportName, promptReportName);
        }

        [Test]
        public void ItRemovesTheAUnderscorePrefixAndReturnsEverythingBeforeTheFinalUnderscoreWhenThereAreThreeUnderscores()
        {
            const string expectedPromptReportName = "Prompt_Name_Test";
            var promptName = string.Format("A_{0}_Alias", expectedPromptReportName);

            var promptReportNameParser = new NativePromptReportNameParser();

            var promptReportName = promptReportNameParser.Parse(promptName);

            Assert.AreEqual(expectedPromptReportName, promptReportName);
        }
    }
}
