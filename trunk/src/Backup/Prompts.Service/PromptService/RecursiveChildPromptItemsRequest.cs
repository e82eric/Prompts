using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public class RecursiveChildPromptItemsRequest
    {
        public string PromptName { get; set; }
        public string ParameterName { get; set; }
        public ParameterValue ParameterValue { get; set; }
    }
}