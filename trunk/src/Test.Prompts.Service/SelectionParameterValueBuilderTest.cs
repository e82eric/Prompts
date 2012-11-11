using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class SelectionParameterValueBuilderTest
    {
        private Mock<IBaseReportInterpreter<IParameterValueBuilder>> _baseReportInterpreter;
        private SelectionParameterValueBuilder _builder;
        private Mock<IPromptSelectionsProvider> _promptSelectionsProvider;

        [SetUp]
        public void Setup()
        {
            _baseReportInterpreter = new Mock<IBaseReportInterpreter<IParameterValueBuilder>>();
            _promptSelectionsProvider = new Mock<IPromptSelectionsProvider>();
            _builder = new SelectionParameterValueBuilder(_baseReportInterpreter.Object, _promptSelectionsProvider.Object);
        }

        [Test]
        public void ItCorrectlyCorrdinatesTheInterpreter()
        {
            var promptSelectionInfos = A.Array(A.PromptSelectionInfo().Build(), A.PromptSelectionInfo().Build());

            var baseReportParameters = A.Array(A.ReportParameter().Build(), A.ReportParameter().Build());

            var prompt1ParameterValues = A.Array(A.ParameterValue().Build(), A.ParameterValue().Build());
            var prompt2ParameterValues = A.Array(A.ParameterValue().Build());

            var parameterValueBuilder1 = Mock.Of<IParameterValueBuilder>();
            var parameterValueBuilder2 = Mock.Of<IParameterValueBuilder>();
            var parameterValueBuilders = A.Array(parameterValueBuilder1, parameterValueBuilder2);

            var promptSelections = Mock.Of<IPromptSelections>(
                s =>
                s.CreateParameterValuesFor(parameterValueBuilder1) == prompt1ParameterValues &&
                s.CreateParameterValuesFor(parameterValueBuilder2) == prompt2ParameterValues);

            _promptSelectionsProvider.Setup(p => p.Get(promptSelectionInfos)).Returns(promptSelections);

            _baseReportInterpreter.Setup(i => i.Get(baseReportParameters)).Returns(parameterValueBuilders);

            var builderParameterValues = _builder.Get(baseReportParameters, promptSelectionInfos);

            builderParameterValues.AssertLength(3);
            builderParameterValues.AssetContians(prompt1ParameterValues);
            builderParameterValues.AssetContians(prompt2ParameterValues);
        }
    }
}
