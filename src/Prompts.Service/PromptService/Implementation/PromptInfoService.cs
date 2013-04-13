using System.Collections.Generic;

namespace Prompts.Service.PromptService.Implementation
{
    public class PromptInfoService
    {
        private readonly IBaseReportParameterService _baseReportParameterService;
        private readonly IBaseReportInterpreter<PromptInfo> _baseReportInterpreter;

        public PromptInfoService(IBaseReportParameterService baseReportParameterService, IBaseReportInterpreter<PromptInfo> baseReportInterpreter)
        {
            _baseReportParameterService = baseReportParameterService;
            _baseReportInterpreter = baseReportInterpreter;
        }

        public IEnumerable<PromptInfo> GetPrompts(string path)
        {
            var baseReportParameters = _baseReportParameterService.GetParametersFor(path);
            return _baseReportInterpreter.Get(baseReportParameters);
        }
    }
}