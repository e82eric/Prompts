namespace Prompts.Service.PromptService
{
    public class GetChildrenResponse
    {
        public PromptLevel PromptLevel { get; set; }
        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }
    }
}