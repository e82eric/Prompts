using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptServiceTest
    {
        private Mock<IBaseReportParameterService> _baseReportParameterService;
        private Mock<IBaseReportInterpreter<PromptInfo>> _baseReportInterpreter;
        private PromptService _service;

        [SetUp]
        public void Setup()
        {
            _baseReportParameterService = new Mock<IBaseReportParameterService>();
            _baseReportInterpreter = new Mock<IBaseReportInterpreter<PromptInfo>>();
            _service = new PromptService(_baseReportParameterService.Object, _baseReportInterpreter.Object);

        }

        [Test]
        public void ItCorrectlyCordinatesTheBaseReportParameterServiceAndInterpreter()
        {
            var request = new PromptsRequest {Path = "Report Path"};

            var parametersForServiceToReturn = A.Array(A.ReportParameter().Build());

            _baseReportParameterService
                .Setup(s => s.GetParametersFor(request.Path))
                .Returns(parametersForServiceToReturn);

            var promptsForInterpreterToReturn = A.Array(A.PromptInfo().Build());

            _baseReportInterpreter.Setup(i => i.Get(parametersForServiceToReturn)).Returns(promptsForInterpreterToReturn);

            var servicePrompts = (IEnumerable<PromptInfo>)_service.OnPost(request);

            Assert.AreEqual(promptsForInterpreterToReturn, servicePrompts);
        }
    }
}
