using System.Linq;
using Prompts.Prompting.ViewModels;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class CasscadingSearchShoppingCartBuilder : IPromptBuilder
    {
        private readonly IPromptBuilder _shoppingCartbuilder;

        public CasscadingSearchShoppingCartBuilder(IPromptBuilder shoppingCartbuilder)
        {
            _shoppingCartbuilder = shoppingCartbuilder;
        }

        public IPrompt BuildFrom(PromptInfo promptInfo)
        {
            var prompt = (IMultiSelectPrompt) _shoppingCartbuilder.BuildFrom(promptInfo);

            if (prompt.SelectedItems.Count() == 1)
            {
                prompt.SearchString = prompt.SelectedItems.Single().Label;
            }

            return prompt;
        }
    }
}