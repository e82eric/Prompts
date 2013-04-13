using Prompts.Prompting.ViewModels.Search;

namespace Prompts.Prompting.Construction
{
    public interface IAsynchronousSearchServiceBuilder
    {
        IAsynchronousSearchService Build(string promptName, string parameterName);
    }
}