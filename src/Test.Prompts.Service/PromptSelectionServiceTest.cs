using System;
using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptSelectionServiceTest
    {
        private Mock<IBaseReportParameterService> _baseParameterService;
        private Mock<ISelectionParameterValueBuilder> _selectionParameterValueBuilder;
        private PromptSelectionService _service;

        [SetUp]
        public void Setup()
        {
            _baseParameterService = new Mock<IBaseReportParameterService>();
            _selectionParameterValueBuilder = new Mock<ISelectionParameterValueBuilder>();
            _service = new PromptSelectionService(
                _baseParameterService.Object
                , _selectionParameterValueBuilder.Object);
        }

        [Test]
        public void ItCorrectlyCordinatesTheSelectionMapperAndTheParameterService()
        {
            var baseReportParameters = A.Array(A.ReportParameter().Build(), A.ReportParameter().Build());
            var parameterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build());
            const string parameterServiceExecutionId = "ExecutionId";

            var request = new SetPromptSelectionsRequest()
                {
                    Path = "Path",
                    PromptSelections = A.Array(A.PromptSelectionInfo().Build(), A.PromptSelectionInfo().Build())
                };

            _baseParameterService
                .Setup(p => p.GetParametersFor(request.Path))
                .Returns(baseReportParameters);

            _selectionParameterValueBuilder
                .Setup(b => b.Get(baseReportParameters, request.PromptSelections))
                .Returns(parameterValues);

            _baseParameterService
                .Setup(s => s.SetParameters(parameterValues))
                .Returns(parameterServiceExecutionId);

            var response = (string)_service.OnPost(request);
            
            Assert.AreEqual(parameterServiceExecutionId, response);
        }
    }
}
