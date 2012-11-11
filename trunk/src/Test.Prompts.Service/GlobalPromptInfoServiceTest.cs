using Moq;
using NUnit.Framework;
using Prompts.Service.PromptService;
using Prompts.Service.PromptService.Implementation;
using Test.Prompts.Service.Infastructure;

namespace Test.Prompts.Service
{
    [TestFixture]
    public class GlobalPromptInfoServiceTest
    {
        private GlobalPromptInfoService _service;
        private Mock<IGlobalPromptInfoProvider> _promptInfoProvider;
        private Mock<IPromptReportParameterService> _promptReportParameterService;
        private Mock<IGlobalPromptBaseReportInfoMapper> _baseReportInfoMapper;

        [SetUp]
        public void Setup()
        {
            _promptReportParameterService = new Mock<IPromptReportParameterService>();
            _promptInfoProvider = new Mock<IGlobalPromptInfoProvider>();
            _baseReportInfoMapper = new Mock<IGlobalPromptBaseReportInfoMapper>();

            _service = new GlobalPromptInfoService(
                _promptReportParameterService.Object
                , _promptInfoProvider.Object
                , _baseReportInfoMapper.Object);
        }

        [Test]
        public void ItCorrectlyCordinatesGettingTheParmaeterFromTheServiceMappingTheBaseInfoAndGettingThePromptInfoFromTheProvider()
        {
            var valueParmaeter = A.ReportParameter().Build();
            var labelParameter = A.ReportParameter().Build();

            var baseReportInfoForMapperToReturn = A.GlobalPromptBaseReportInfo()
                .WithName("Base Report Info")
                .Build();

            _baseReportInfoMapper
                .Setup(m => m.Map(valueParmaeter, labelParameter))
                .Returns(baseReportInfoForMapperToReturn);

            var promptReportParameterForSerivceToReturn = A.Array(A.ReportParameter("Parameter 1"), A.ReportParameter("Parameter 2"));
            _promptReportParameterService
                .Setup(s => s.GetParametersFor(baseReportInfoForMapperToReturn.Name))
                .Returns(promptReportParameterForSerivceToReturn);

            var promptInfoForProviderToReturn = A.PromptInfo().WithName("Prompt Info").Build();
            _promptInfoProvider
                .Setup(p => p.GetPromptInfo(baseReportInfoForMapperToReturn, promptReportParameterForSerivceToReturn))
                .Returns(promptInfoForProviderToReturn);

            var promptInfo = _service.Get(valueParmaeter, labelParameter);
            Assert.AreEqual(promptInfoForProviderToReturn, promptInfo);
        }
    }
}
