using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService.Implementation
{
    public class EmbeddedPromptParameterValueProvider : IEmbeddedPromptProvider<IParameterValueBuilder>
    {
        public IParameterValueBuilder Get(ReportParameter parameter)
        {
            return new EmbeddedPromptParameterValueBuilder(parameter);
        }
    }
}