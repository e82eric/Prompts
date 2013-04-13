using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Exceptions;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class PromptInfoProviderTest
    {
        private GlobalPromptInfoProvider _provider;
        private Mock<ICasscadingPromptInfoProvider> _casscadingPromptInfoProvider;
        private Mock<IHierarchyPromptInfoProvider> _hierarchyPromptInfoProvider;
        private Mock<ISingleLevelPromptInfoProvider> _singleLevelPromptInfoProvider;
        private string _recursiveHierarchyPrefix;
        private Mock<IHierarchyPromptInfoProvider> _recursiveHierarchyPromptInfoProvider;

        [SetUp]
        public void Setup()
        {
            _singleLevelPromptInfoProvider = new Mock<ISingleLevelPromptInfoProvider>();
            _hierarchyPromptInfoProvider = new Mock<IHierarchyPromptInfoProvider>();
            _casscadingPromptInfoProvider = new Mock<ICasscadingPromptInfoProvider>();
            _recursiveHierarchyPromptInfoProvider = new Mock<IHierarchyPromptInfoProvider>();

            _recursiveHierarchyPrefix = "RecursivePrefix";
            _provider = new GlobalPromptInfoProvider(
                _singleLevelPromptInfoProvider.Object
                , _hierarchyPromptInfoProvider.Object
                , _casscadingPromptInfoProvider.Object
                , _recursiveHierarchyPromptInfoProvider.Object
                , _recursiveHierarchyPrefix);
        }

        [Test]
        public void UsesSingleLevelBuilderWhenThereIsOnlyOneParameter()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo().Build();
            var promptReportParameters = A.Array(A.ReportParameter("Parameter 1"));

            var promptInfoFromSingleLevelProvider = A.PromptInfo().WithName("Prompt Info").Build();
            _singleLevelPromptInfoProvider
                .Setup(p => p.GetPromptInfo(baseReportInfo, promptReportParameters[0]))
                .Returns(promptInfoFromSingleLevelProvider);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, promptReportParameters);
            Assert.AreEqual(promptInfoFromSingleLevelProvider, promptInfo);
        }

        [Test]
        public void UsesHierarchyBuilderWhenThereAreMoreThanOneParameterAndTheFirstIsNotEmpty()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo().Build();
            var promptReportParameters = A.Array(
                A.ReportParameter().WithValidValues(A.ValidValue().Build()).Build()
                , A.ReportParameter().Build());

            var promptInfoFromSingleLevelProvider = A.PromptInfo().WithName("Prompt Info").Build();
            _hierarchyPromptInfoProvider
                .Setup(p => p.GetPromptInfo(baseReportInfo, promptReportParameters))
                .Returns(promptInfoFromSingleLevelProvider);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, promptReportParameters);
            Assert.AreEqual(promptInfoFromSingleLevelProvider, promptInfo);
        }

        [Test]
        public void UsesSearchBuilderWhenThereAreMoreThanOneParameterAndTheFirsEmptyAndTheNameDoesNotStartWithTheRecursivePrefix()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo().Build();
            var promptReportParameters = A.Array(
                A.ReportParameter().WithValidValues(null).Build()
                , A.ReportParameter().Build());

            var promptInfoFromSingleLevelProvider = A.PromptInfo().WithName("Prompt Info").Build();
            _casscadingPromptInfoProvider
                .Setup(p => p.GetPromptInfo(baseReportInfo, promptReportParameters[0], promptReportParameters[1]))
                .Returns(promptInfoFromSingleLevelProvider);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, promptReportParameters);
            Assert.AreEqual(promptInfoFromSingleLevelProvider, promptInfo);
        }

        [Test]
        public void ItUsesTheRecursiveTreeBuilderWhenThereAreMoreThanOneParameterAndTheFirsEmptyAndTheNameDoesStartWithTheRecursivePrefix()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo().WithName(string.Format("{0}_Prompt1", _recursiveHierarchyPrefix)).Build();
            var promptReportParameters = A.Array(
                A.ReportParameter().WithValidValues(null).Build()
                , A.ReportParameter().Build());

            var promptInfoFromSingleLevelProvider = A.PromptInfo().WithName("Prompt Info").Build();
            _recursiveHierarchyPromptInfoProvider
                .Setup(p => p.GetPromptInfo(baseReportInfo, promptReportParameters))
                .Returns(promptInfoFromSingleLevelProvider);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, promptReportParameters);
            Assert.AreEqual(promptInfoFromSingleLevelProvider, promptInfo);
        }

        [Test]
        public void ThrowsExceptionWhenTheParametersAreNull()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo().WithName("Prompt Name").Build();

            var expectedMessage = string.Format(
                "An error occured building Global Prompt '{0}', There were no parameters"
                , baseReportInfo.Name);

            ExceptionAssert.Throws<PromptInfoProviderException>(expectedMessage
                , () => _provider.GetPromptInfo(baseReportInfo, null));
        }

        //TODO: Add more tests from unknown prompt report configurations
    }
}