namespace Prompts.Prompting.ViewModels.Search
{
    public interface ISearchProvider<out T>
    {
        T CreateContainsSearch(string searchString);
        T CreateEndsWithSearch(string searchString);
        T CreateEqualsSearch(string searchString);
        T CreateNullSearch();
        T CreateStartsWithSearch(string searchString);
    }
}