namespace Prompts.Prompting.ViewModels.Search.Implementation
{
    public interface ISearchStringParser<out T>
    {
        T Parse(string searchExpression);
    }
}