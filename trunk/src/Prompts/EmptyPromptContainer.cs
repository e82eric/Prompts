using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;

namespace Prompts
{
    public class EmptyPromptContainer
    {
        public virtual IPromptBuilder Create()
        {
            return new EmptyPromptBuilder();
        }
    }
}
