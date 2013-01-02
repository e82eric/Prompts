using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class BaseReportInterpreterTest
    {
        private Mock<IGlobalPromptProvider<IParameterValueBuilder>> _globalPromptInfoProvider;
        private Mock<IEmbeddedPromptProvider<IParameterValueBuilder>> _embeddedPromptInfoProvider;
        private BaseReportInterpreter<IParameterValueBuilder> _interpreter;

        [SetUp]
        public void Setup()
        {
            _globalPromptInfoProvider = new Mock<IGlobalPromptProvider<IParameterValueBuilder>>();
            _embeddedPromptInfoProvider = new Mock<IEmbeddedPromptProvider<IParameterValueBuilder>>();

            _interpreter = new BaseReportInterpreter<IParameterValueBuilder>(_globalPromptInfoProvider.Object, _embeddedPromptInfoProvider.Object);
        }

        [Test]
        public void ItSendsTheParameterToTheEmbeddedPromptBuilderWhenTheFollowingParameterDoesNotStartWithTheSameName()
        {
            var parameter1 = A.ReportParameter()
                .WithName("Prompt1")
                .WithValidValues(null)
                .Build();
            
            var parameter2 = A.ReportParameter()
                .WithName("Prompt2")
                .WithValidValues(A.Array(A.ValidValue().Build()))
                .Build();
            
            var parameters = A.Array(parameter1, parameter2);

            var parameterValueBuilder1 = Mock.Of<IParameterValueBuilder>();
            var parameterValueBuilder2 = Mock.Of<IParameterValueBuilder>();

            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter1)).Returns(parameterValueBuilder1);
            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter2)).Returns(parameterValueBuilder2);

            var builders = _interpreter.Get(parameters);
            builders.AssetItemsAndLength(parameterValueBuilder1, parameterValueBuilder2);
        }

        [Test]
        public void ItSendsBothParametersToTheEmbeddedPromptBuilderWhenTheSecondParametersNameStartWithTheFirstParametersNameButItsValidValuesAreNotEmpty()
        {
            const string promptName = "PromptName";

            var parameter1 = A.ReportParameter()
                .WithName(promptName)
                .WithValidValues(null)
                .Build();

            var parameter2 = A.ReportParameter()
                .WithName(string.Format("{0}_Label", promptName))
                .WithValidValues(A.ValidValue().Build())
                .Build();

            var parameters = A.Array(parameter1, parameter2);

            var builder1 = Mock.Of<IParameterValueBuilder>();
            var builder2 = Mock.Of<IParameterValueBuilder>();

            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter1)).Returns(builder1);
            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter2)).Returns(builder2);

            var builders = _interpreter.Get(parameters);

            _globalPromptInfoProvider.Verify(p => p.Get(It.IsAny<ReportParameter>(), It.IsAny<ReportParameter>()), Times.Exactly(0));

            builders.AssetItemsAndLength(builder1, builder2);
        }

        [Test]
        public void ItSendsTheParameterToTheEmbeddedPromptBuilderWhenItIsTheLastParameter()
        {
            var parameter1 = A.ReportParameter()
                .WithName("Prompt1")
                .WithValidValues(A.Array(A.ValidValue().Build()))
                .Build();

            var parameter2 = A.ReportParameter()
                .WithName("Prompt2")
                .WithValidValues(null)
                .Build();

            var parameters = A.Array(parameter1, parameter2);

            var parameterValueBuilder1 = Mock.Of<IParameterValueBuilder>();
            var parameterValueBuilder2 = Mock.Of<IParameterValueBuilder>();

            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter1)).Returns(parameterValueBuilder1);
            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter2)).Returns(parameterValueBuilder2);

            var builders = _interpreter.Get(parameters);
            builders.AssetItemsAndLength(parameterValueBuilder1, parameterValueBuilder2);
        }

        [Test]
        public void ItSendTheFirstParameterToTheEmbeddedPromptBuilderWhenItsValidValuesAreNotNull()
        {
            var parameter1 = A.ReportParameter().WithValidValues(A.Array(A.ValidValue("Value 1"))).Build();
            var parmaeters = A.Array(parameter1);

            var providerBuilder = Mock.Of<IParameterValueBuilder>();

            _embeddedPromptInfoProvider.Setup(p => p.Get(parameter1)).Returns(
                providerBuilder);

            var interpreterBuilders = _interpreter.Get(parmaeters).AssertSingle();

            Assert.AreEqual(providerBuilder, interpreterBuilders);
        }

        [Test]
        public void ItSendsTheFirstAndSecondParmaeterToTheEmbeddedPromptBuilderWhenBothHaveValidValues()
        {
            var parameter1 = A.ReportParameter().WithValidValues(new[] { A.ValidValue().Build() }).Build();
            var parameter2 = A.ReportParameter().WithValidValues(new[] { A.ValidValue().Build() }).Build();
            var parmaeters = A.Array(parameter1, parameter2);

            var prompt1Builder = Mock.Of<IParameterValueBuilder>();
            var prompt2Builder = Mock.Of<IParameterValueBuilder>();

            _embeddedPromptInfoProvider.Setup(s => s.Get(parameter1)).Returns(prompt1Builder);
            _embeddedPromptInfoProvider.Setup(s => s.Get(parameter2)).Returns(prompt2Builder);

            var interpreterBuilders = _interpreter.Get(parmaeters);
            interpreterBuilders.AssertEqual(A.Array(prompt1Builder, prompt2Builder));
        }

        [Test]
        public void ItSendsTheParameterAndItsFollowingParameterToTheGlobalPromptBuilderWhenItsValidValuesAreEmptyAndTheFollwingParametersNameStartsWithTheFirstParametersName()
        {
            const string promptName = "PromptName";

            var emptyParameter1 = A.ReportParameter()
                .WithName(promptName)
                .WithValidValues(null)
                .Build();

            var labelParameter1 = A.ReportParameter()
                .WithName(string.Format("{0}_Label", promptName))
                .WithValidValues(null)
                .Build();

            var parameters = A.Array(emptyParameter1, labelParameter1);

            var prompt1Builder = Mock.Of<IParameterValueBuilder>();

            _globalPromptInfoProvider.Setup(p => p.Get(emptyParameter1, labelParameter1)).Returns(prompt1Builder);

            var builders = _interpreter.Get(parameters);
            builders.AssetItemsAndLength(prompt1Builder);
        }

        [Test]
        public void ReturnsEmptyCollectionWhenParametersAreNull()
        {
            ReportParameter[] parameters = null;

            _interpreter.Get(parameters).AssertLength(0);
        }

        [Test]
        public void ItIgnoresTheParameterIfPromptIsBlank()
        {
            var parameter1 = A.ReportParameter()
                .WithName("Prompt1")
                .WithValidValues(null)
                .WithPrompt(string.Empty)
                .Build();

            var parameters = A.Array(parameter1);

            var builders = _interpreter.Get(parameters);

            _embeddedPromptInfoProvider.Verify(p => p.Get(It.IsAny<ReportParameter>()), Times.Exactly(0));
            _globalPromptInfoProvider.Verify(
                p => p.Get(It.IsAny<ReportParameter>(), It.IsAny<ReportParameter>()), Times.Exactly(0));

            builders.AssertLength(0);
        }
    }
}
