using Prompts.Prompting.ViewModels.Search;

namespace Prompts.Prompting.Construction
{
    public interface ISearchServiceBuilder
    {
        ISearchService Build(ISearchablePromptItemCollection searchablePromptItemCollection);
    }
}