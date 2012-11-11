using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Prompting.ViewModels.Search.Implementation;

namespace Prompts
{
    public class CasscadingSearchContainer
    {
        private const string SearchWildcard = "*";

        public IPromptBuilder Create()
        {
            return new CasscadingSearchShoppingCartBuilder(
                new ShoppingCartBuilder<ISearchablePromptItem>(
                    new SearchablePromptItemProvider(),
                    new CasscadingSearchProvider(CreateAsynchronousSearchService())));
        }

        private static IAsynchronousSearchServiceBuilder CreateAsynchronousSearchService()
        {
            return new AsynchronousSearchServiceBuilder(
                SearchWildcard
                , CreateChildPromptItemService()
                , new SearchProvider());
        }

        private static IChildPromptItemsService CreateChildPromptItemService()
        {
            return new ChildPromptItemsService(
                ServiceInjector.Inject<IChildPromptLevelServiceClient>()
                , new PromptItemCollectionBuilder());
        }
    }
}
