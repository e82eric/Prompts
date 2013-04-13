using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.Construction.Implementation
{
    public class SearchablePromptItemProvider : IPromptItemProvider<ISearchablePromptItem>
    {
        public ISearchablePromptItem Get(string promptName, string parameterName, ValidValue validValue)
        {
            return new SearchablePromptItem(promptName, parameterName, validValue, false);
        }
    }
}
