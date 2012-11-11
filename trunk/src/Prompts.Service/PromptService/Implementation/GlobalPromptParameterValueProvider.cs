using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class GlobalPromptParameterValueProvider : IGlobalPromptProvider<IParameterValueBuilder>
    {
        public IParameterValueBuilder Get(ReportParameter valueParmaeter, ReportParameter labelParameter)
        {
            return new GlobalPromptParameterValueBuilder(valueParmaeter, labelParameter);
        }
    }
}