using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptReportParameterServiceTest
    {
        private Mock<IReportServerFolder> _reportServerFolder;
        private Mock<IReportExecutionService> _reportExecutionService;
        private Mock<IPromptReportNameParser> _promptReportNameParser;
        private PromptReportParameterService _parameterSerivce;

        [SetUp]
        public void Setup()
        {
            _reportServerFolder = new Mock<IReportServerFolder>();
            _reportExecutionService = new Mock<IReportExecutionService>();
            _promptReportNameParser = new Mock<IPromptReportNameParser>();
            
            _parameterSerivce = new PromptReportParameterService(
                _reportServerFolder.Object,
                _reportExecutionService.Object,
                _promptReportNameParser.Object);
        }

        [Test]
        public void ItReturnsTheParametersFromThePromptService()
        {
            const string promptName = "Prompt Name";
            const string path = "Path";
            const string promptReportName = "Prompt Report Name";

            _promptReportNameParser.Setup(p => p.Parse(promptName)).Returns(promptReportName);

            var parameters = A.Array(A.ReportParameter().Build(), A.ReportParameter().Build());

            var executionInfo = new ExecutionInfo2 { Parameters = parameters };

            _reportServerFolder.Setup(f => f.GetFullPathFor(promptReportName)).Returns(path);
            _reportExecutionService.Setup(s => s.LoadReport2(path, null)).Returns(executionInfo);

            var serviceParameters = _parameterSerivce.GetParametersFor(promptName);

            Assert.AreEqual(parameters, serviceParameters);
        }

        [Test]
        public void ItReturnsTheParametersFromThePromptService2()
        {
            const string promptName = "Prompt Name";
            const string path = "Path";
            const string promptReportName = "Prompt Report Name";
            var parmaterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build());
            var parameters = A.Array(A.ReportParameter().Build(), A.ReportParameter().Build());
            var executionInfo = new ExecutionInfo2 { Parameters = parameters };

            _promptReportNameParser
                .Setup(p => p.Parse(promptName))
                .Returns(promptReportName);

            _reportServerFolder
                .Setup(f => f.GetFullPathFor(promptReportName))
                .Returns(path);

            _reportExecutionService
                .Setup(s => s.SetExecutionParameters2(parmaterValues, null))
                .Returns(executionInfo);

            var serviceParameters = _parameterSerivce.GetParametersFor(promptName, parmaterValues);

            _reportExecutionService.Verify(s => s.LoadReport2(path, null), Times.Exactly(1));

            Assert.AreEqual(parameters, serviceParameters);
        }
    }
}
