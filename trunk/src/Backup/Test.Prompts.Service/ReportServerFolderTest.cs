using NUnit.Framework;
using Prompts.Service.PromptService.Implementation;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class ReportServerFolderTest
    {
        [Test]
        public void GetFullPathForReport()
        {
            const string folderPath = "Folder Path";
            const string reportName = "Report Name";
            const string expectedFullPath = "Folder Path/Report Name";

            var reportServerFolder = new ReportServerFolder(folderPath);
            var actualFullPath = reportServerFolder.GetFullPathFor(reportName);

            Assert.AreEqual(expectedFullPath, actualFullPath);
        }
    }
}
