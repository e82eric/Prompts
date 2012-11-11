using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prompts.Prompting.ViewModels;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.Construction
{
    public interface IPromptItemCollectionProvider
    {
        ObservableCollection<ISearchablePromptItem> Get(
            string promptName, 
            string parameterName, 
            IEnumerable<ValidValue> validValues);
    }
}
