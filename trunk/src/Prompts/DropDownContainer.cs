using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.ViewModels;

namespace Prompts
{
    public class DropDownContainer
    {
        public IPromptBuilder Create()
        {
            return new SingleSelectPromptBuilder<ISearchablePromptItem>(
                new SearchablePromptItemProvider(), 
                CreateDropDownProvider());
        }

        protected virtual ISingleSelectPromptProvider<ISearchablePromptItem> CreateDropDownProvider()
        {
            return new DropDownProvider();
        }
    }
}
