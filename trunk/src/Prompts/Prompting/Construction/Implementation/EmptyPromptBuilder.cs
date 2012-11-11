using System.Linq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.PromptService;

namespace Prompts.Prompting.Construction.Implementation
{
    public class EmptyPromptBuilder : IPromptBuilder
    {
        public IPrompt BuildFrom(PromptInfo promptInfo)
        {
            string defaultText = null;
            if(promptInfo.DefaultValues != null)
            {
                if (promptInfo.DefaultValues.Count() == 1)
                {
                    defaultText = promptInfo.DefaultValues.Single().Value;
                }
            }

            return new EmptyPrompt(promptInfo.Name, promptInfo.Label) {Text = defaultText};
        }
    }
}
