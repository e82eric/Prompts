using ServiceStack.ServiceInterface;

namespace Prompts.Service.PromptService
{
    public class PromptService : RestServiceBase<PromptsRequest>
    {
        private readonly IBaseReportParameterService _baseReportParameterService;
        private readonly IBaseReportInterpreter<PromptInfo> _baseReportInterpreter;

        public PromptService(
            IBaseReportParameterService baseReportParameterService, 
            IBaseReportInterpreter<PromptInfo> baseReportInterpreter)
        {
            _baseReportInterpreter = baseReportInterpreter;
            _baseReportParameterService = baseReportParameterService;
        }

        public override object OnPost(PromptsRequest request)
        {
            var baseReportParameters = _baseReportParameterService.GetParametersFor(request.Path);
            return _baseReportInterpreter.Get(baseReportParameters);
        }
    }
}
