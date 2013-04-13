using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.Construction.Implementation
{
    public class RootTreeNodeProvider : IPromptItemProvider<ITreeNode>
    {
        private readonly IChildTreeNodeService _childTreeNodeService;

        public RootTreeNodeProvider(IChildTreeNodeService childTreeNodeService)
        {
            _childTreeNodeService = childTreeNodeService;
        }

        public ITreeNode Get(string promptName, string parameterName, ValidValue validValue)
        {
            return new RootAsynchronousTreeNode(
                promptName, 
                parameterName, 
                validValue, 
                _childTreeNodeService);
        }
    }
}
