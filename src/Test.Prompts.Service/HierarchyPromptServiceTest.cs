using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class HierarchyPromptServiceTest
    {
        private Mock<IPromptReportParameterService> _parameterService;
        private Mock<IHierarchyPromptProvider> _hierarchyPromptProvider;
        private HierarchyPromptService _service;

        [SetUp]
        public void Setup()
        {
            _parameterService = new Mock<IPromptReportParameterService>();
            _hierarchyPromptProvider = new Mock<IHierarchyPromptProvider>();
            _service = new HierarchyPromptService(_parameterService.Object, _hierarchyPromptProvider.Object);
        }

        [Test]
        public void ItGetsTheParametersFromTheParmaeterServiceAndThenGetTheHierarchyFromThePrompt()
        {
            const string promptName = "Path";
            var parameterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build());
            var parametersForServiceToReturn = A.Array(A.ReportParameter().Build(), A.ReportParameter().Build());
            var providerHierarchy = Mock.Of<IHierarchyPrompt>();

            _parameterService.Setup(s => s.GetParametersFor(promptName, parameterValues)).Returns(parametersForServiceToReturn);
            _hierarchyPromptProvider.Setup(p => p.Get(parametersForServiceToReturn)).Returns(providerHierarchy);

            var serviceHierarhcy = _service.GetHierarchyPrompt(promptName, parameterValues);

            Assert.AreEqual(providerHierarchy, serviceHierarhcy);
        }
    }
}
