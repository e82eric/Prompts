using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class SingleLevelPromptInfoProviderTest
    {
        private Mock<IPromptTypeProvider> _promptTypeProvider;
        private Mock<IStrictDefaultValuesProvider> _defaultValueProvider;
        private Mock<IPromptLevelProvider> _promptLevelProvider;
        private SingleLevelPromptInfoProvider _provider;

        [SetUp]
        public void Setup()
        {
            _promptTypeProvider = new Mock<IPromptTypeProvider>();
            _defaultValueProvider = new Mock<IStrictDefaultValuesProvider>();
            _promptLevelProvider = new Mock<IPromptLevelProvider>();

            _provider = new SingleLevelPromptInfoProvider(_promptTypeProvider.Object, _promptLevelProvider.Object
                                                          , _defaultValueProvider.Object);
        }

        [Test]
        public void ItGetTheDefaultValuesFromTheDefaultValueProviderAndThePromptLevelFromThePromptLevelProvider()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithValueParameterDefaults("Default 1", "Default 2")
                .WithName("Prompt Name")
                .Build();

            var parmaeter = A.ReportParameter("Parameter 1");

            var promptLevelForProviderToReturn = A.PromptLevel()
                .WithAvailableItems(A.ValidValue("Value 1"), A.ValidValue("Value 2"))
                .WithParameterName("Parameter Name")
                .Build();

            _promptLevelProvider.Setup(p => p.GetPromptLevel(parmaeter)).Returns(promptLevelForProviderToReturn);

            var defaultValuesForProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));
            _defaultValueProvider.Setup(
                p =>
                p.GetDefaultValues(promptLevelForProviderToReturn, baseReportInfo.ValueParameterDefaults))
                .Returns(defaultValuesForProviderToReturn);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, parmaeter);
            Assert.AreEqual(defaultValuesForProviderToReturn, promptInfo.DefaultValues);
            Assert.AreEqual(promptLevelForProviderToReturn, promptInfo.PromptLevelInfo);
        }

        [Test]
        public void ItGetsThePromptTypeFromTheProvider()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithSelectionType(SelectionType.MultiSelect)
                .WithName("Prompt Name").Build();
            var parmaeter = A.ReportParameter("Parameter 1");

            const PromptType promptTypeForProviderToReturn = PromptType.Tree;
            _promptTypeProvider.Setup(p => p.GetPromptType(baseReportInfo.SelectionType))
                .Returns(promptTypeForProviderToReturn);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, parmaeter);
            Assert.AreEqual(promptTypeForProviderToReturn, promptInfo.PromptType);
        }

        [Test]
        public void ItGetsTheNameAndLabelFromTheGlobalPrompt()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithName("Prompt Name")
                .WithLabel("Prompt Label")
                .Build();

            var parmaeter = A.ReportParameter("Parameter 1");

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, parmaeter);
            Assert.AreEqual(baseReportInfo.Name, promptInfo.Name);
            Assert.AreEqual(baseReportInfo.Label, promptInfo.Label);
        }
    }
}