using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class CasscadingPromptInfoProviderTest
    {
        private CasscadingPromptInfoProvider _provider;
        private Mock<IPromptTypeProvider> _promptTypeProvider;
        private Mock<ICascadingSearchPromptLevelProvider> _promptLevelProvider;
        private Mock<ICasscadingSearchValidator> _promptReportValidator;
        private Mock<ICascadingSearchDefaultValueProvider> _defaultValueProvider;

        [SetUp]
        public void Setup()
        {
            _promptTypeProvider = new Mock<IPromptTypeProvider>();
            _defaultValueProvider = new Mock<ICascadingSearchDefaultValueProvider>();
            _promptLevelProvider = new Mock<ICascadingSearchPromptLevelProvider>();
            _promptReportValidator = new Mock<ICasscadingSearchValidator>();
            _provider = new CasscadingPromptInfoProvider(
                _promptTypeProvider.Object,
                _promptLevelProvider.Object,
                _defaultValueProvider.Object,
                _promptReportValidator.Object);
        }
        
        [Test]
        public void ItCorrectlyCallsThePromptReportValidator()
        {
            var searchParmaeter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(searchParmaeter.Name).Build();

            var baseReportInfo = A.GlobalPromptBaseReportInfo().WithName("Prompt Name").Build();

            _provider.GetPromptInfo(baseReportInfo, searchParmaeter, resultParameter);

            _promptReportValidator.Verify(v => v.Validate(baseReportInfo.Name, searchParmaeter, resultParameter)
                , Times.Exactly(1));
        }

        [Test]
        public void ItUsesTheBaseReportInfosNameForName()
        {
            var searchParmaeter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(searchParmaeter.Name).Build();
            var baseReportInfo = A.GlobalPromptBaseReportInfo().WithName("Prompt Name").Build();

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, searchParmaeter, resultParameter);
            Assert.AreEqual(baseReportInfo.Name, promptInfo.Name);
        }

        [Test]
        public void ItUsesTheGlobalPromptsLabelForLabel()
        {
            var searchParmaeter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(searchParmaeter.Name).Build();
            var baseReportInfo = A.GlobalPromptBaseReportInfo().WithLabel("Prompt Label").Build();

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, searchParmaeter, resultParameter);
            Assert.AreEqual(baseReportInfo.Label, promptInfo.Label);
        }

        [Test]
        public void ItGetsThePromptTypeFromThePromptTypeProvider()
        {
            var searchParmaeter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(searchParmaeter.Name).Build();
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithSelectionType(SelectionType.MultiSelect)
                .Build();

            _promptTypeProvider.Setup(p => p.GetPromptType(baseReportInfo.SelectionType))
                .Returns(PromptType.CasscadingSearch);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, searchParmaeter, resultParameter);
            Assert.AreEqual(PromptType.CasscadingSearch, promptInfo.PromptType);
        }

        [Test]
        public void ItGetsTheDefaultValuesFromTheDefaultValueProvider()
        {
            var searchParmaeter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(searchParmaeter.Name).Build();
            var baseReportInfo = A.GlobalPromptBaseReportInfo().Build();

            var defaultValuesForProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));

            _defaultValueProvider.Setup(
                p => p.Get(
                    baseReportInfo.Name,
                    searchParmaeter.Name,
                    baseReportInfo.ValueParameterDefaults,
                    baseReportInfo.LabelParameterDefaults))
                .Returns(defaultValuesForProviderToReturn);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, searchParmaeter, resultParameter);
            Assert.AreEqual(defaultValuesForProviderToReturn, promptInfo.DefaultValues);
        }

        [Test]
        public void ItGetsThePromptLevelFromThePromptLevelProvider()
        {
            var searchParmaeter = A.ReportParameter().WithValidValues(null).Build();
            var resultParameter = A.ReportParameter().WithDependencies(searchParmaeter.Name).Build();
            var baseReportInfo = A.GlobalPromptBaseReportInfo().Build();

            var promptLevel = A.PromptLevel().WithParameterName("Parameter Name").Build();

            var defaultValuesForProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));

            _defaultValueProvider.Setup(
                p => p.Get(
                    baseReportInfo.Name,
                    searchParmaeter.Name,
                    baseReportInfo.ValueParameterDefaults,
                    baseReportInfo.LabelParameterDefaults))
                .Returns(defaultValuesForProviderToReturn);

            _promptLevelProvider.Setup(p => p.GetPromptLevel(searchParmaeter.Name, defaultValuesForProviderToReturn)).Returns(promptLevel);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, searchParmaeter, resultParameter);
            Assert.AreEqual(promptLevel, promptInfo.PromptLevelInfo);
        }
    }
}
