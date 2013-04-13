namespace Prompts.Service.PromptService
{
    public interface IDefaultValueProvider
    {
        DefaultValue Get(string value, string label);
    }
}