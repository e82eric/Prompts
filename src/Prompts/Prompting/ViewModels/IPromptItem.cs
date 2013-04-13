namespace Prompts.Prompting.ViewModels
{
    public interface IPromptItem
    {
        string Label { get; }
        string Value { get; }
        string PromptName { get; }
        string ParameterName { get; }
        bool IsDefaultAll { get; set; }
    }
}