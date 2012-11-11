using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class GlobalPromptInfoService : IGlobalPromptProvider<PromptInfo>
    {
        private readonly IPromptReportParameterService _promptReportParameterService;
        private readonly IGlobalPromptInfoProvider _globalPromptInfoProvider;
        private readonly IGlobalPromptBaseReportInfoMapper _globalPromptBaseReportInfoMapper;

        public GlobalPromptInfoService(
            IPromptReportParameterService promptReportParameterService, 
            IGlobalPromptInfoProvider globalPromptInfoProvider, 
            IGlobalPromptBaseReportInfoMapper globalPromptBaseReportInfoMapper)
        {
            _globalPromptBaseReportInfoMapper = globalPromptBaseReportInfoMapper;
            _globalPromptInfoProvider = globalPromptInfoProvider;
            _promptReportParameterService = promptReportParameterService;
        }

        public PromptInfo Get(ReportParameter valueParameter, ReportParameter labelParameter)
        {
            var baseReportInfo = _globalPromptBaseReportInfoMapper.Map(valueParameter, labelParameter);
            var promptReportParameters = _promptReportParameterService.GetParametersFor(baseReportInfo.Name);
            return _globalPromptInfoProvider.GetPromptInfo(baseReportInfo, promptReportParameters);
        }
    }
}