using Prompts.Service.ReportExecution;

namespace Prompts.Service.PromptService
{
    public class ChildPromptItemsRequest
    {
        public string PromptName { get; set; }
        public string ParameterName { get; set; }
        public ParameterValue[] ParameterValues { get; set; }
    }
}