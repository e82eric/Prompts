namespace Prompts.Prompting.ViewModels.Search.Implementation
{
    public class SearchProvider : ISearchProvider<ISearch>
    {
        public ISearch CreateContainsSearch(string searchString)
        {
            return new ContainsSearch(searchString);
        }
        public ISearch CreateEndsWithSearch(string searchString)
        {
            return new EndsWithSearch(searchString);
        }
        public ISearch CreateEqualsSearch(string searchString)
        {
            return new EqualsSearch(searchString);
        }
        public ISearch CreateNullSearch()
        {
            return new NullSearch(string.Empty);
        }
        public ISearch CreateStartsWithSearch(string searchString)
        {
            return new StartsWithSearch(searchString);
        }
    }
}
