using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class HierarchyPromptInfoProviderTest
    {
        private Mock<IPromptTypeProvider> _promptTypeProvider;
        private Mock<IStrictDefaultValuesProvider> _defaultValueProvider;
        private Mock<IPromptLevelProvider> _promptLevelProvider;
        private Mock<IHierarchyPromptReportValidator> _hierarchyValidator;
        private HierarchyPromptInfoProvider _provider;

        [SetUp]
        public void Setup()
        {
            _promptTypeProvider = new Mock<IPromptTypeProvider>();
            _defaultValueProvider = new Mock<IStrictDefaultValuesProvider>();
            _promptLevelProvider = new Mock<IPromptLevelProvider>();
            _hierarchyValidator = new Mock<IHierarchyPromptReportValidator>();

            _provider = new HierarchyPromptInfoProvider(_promptTypeProvider.Object, _promptLevelProvider.Object
                                                        , _defaultValueProvider.Object, _hierarchyValidator.Object);
        }

        [Test]
        public void ItCallsTheValidator()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo().WithName("Promp Name").Build();
            
            var parmaeters = A.Array(A.ReportParameter("Parameter 1"), A.ReportParameter("Parameter 2"));

            _provider.GetPromptInfo(baseReportInfo, parmaeters);
            _hierarchyValidator.Verify(v => v.Validate(baseReportInfo.Name, parmaeters));
        }

        [Test]
        public void ItGetTheDefaultValuesFromTheDefaultValueProviderAndThePromptLevelFromThePromptLevelProvider()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithValueParameterDefaults("Default 1", "Default 2")
                .WithName("Promp Name")
                .Build();

            var parmaeters = A.Array(A.ReportParameter("Parameter 1"), A.ReportParameter("Parameter 2"));

            var promptLevelForProviderToReturn = A.PromptLevel().Build();

            _promptLevelProvider.Setup(p => p.GetPromptLevel(parmaeters[0])).Returns(promptLevelForProviderToReturn);

            var defaultValuesForProviderToReturn = A.Array(A.DefaultValue("Value 1"), A.DefaultValue("Value 2"));
            _defaultValueProvider.Setup(
                p => 
                p.GetDefaultValues(promptLevelForProviderToReturn, baseReportInfo.ValueParameterDefaults))
                .Returns(defaultValuesForProviderToReturn);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, parmaeters);
            Assert.AreEqual(defaultValuesForProviderToReturn, promptInfo.DefaultValues);
            Assert.AreEqual(promptLevelForProviderToReturn, promptInfo.PromptLevelInfo);
        }

        [Test]
        public void ItGetsThePromptTypeFromTheProvider()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithName("Promp Name")
                .WithSelectionType(SelectionType.MultiSelect)
                .Build();

            var parmaeters = A.Array(A.ReportParameter("Parameter 1"), A.ReportParameter("Parameter 2"));

            const PromptType promptTypeForProviderToReturn = PromptType.Tree;
            _promptTypeProvider.Setup(p => p.GetPromptType(baseReportInfo.SelectionType))
                .Returns(promptTypeForProviderToReturn);

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, parmaeters);
            Assert.AreEqual(promptTypeForProviderToReturn, promptInfo.PromptType);
        }

        [Test]
        public void ItGetsTheNameAndLabelFromTheGlobalPrompt()
        {
            var baseReportInfo = A.GlobalPromptBaseReportInfo()
                .WithName("Prompt Name")
                .WithLabel("Prompt Label")
                .Build();

            var parmaeters = A.Array(A.ReportParameter("Parameter 1"), A.ReportParameter("Parameter 2"));

            var promptInfo = _provider.GetPromptInfo(baseReportInfo, parmaeters);
            Assert.AreEqual(baseReportInfo.Name, promptInfo.Name);
            Assert.AreEqual(baseReportInfo.Label, promptInfo.Label);
        }
    }
}
