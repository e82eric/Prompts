using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels;
using Prompts.Service.ReportExecution;

namespace Prompts
{
    public class ShoppingCartContainer
    {
        public IPromptBuilder Create()
        {
            return new ShoppingCartBuilder<ISearchablePromptItem>(
                InjectSearchablePromptItemProvider()
                , new SearchableShoppingCartProvider());
        }

        private static IPromptItemProvider<ISearchablePromptItem> InjectSearchablePromptItemProvider()
        {
            return new SearchablePromptItemProvider();
        }
    }
}
