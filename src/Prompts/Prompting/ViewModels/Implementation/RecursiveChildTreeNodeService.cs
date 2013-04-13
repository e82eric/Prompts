using System;
using System.Collections.ObjectModel;
using Prompts.Prompting.Construction;
using Prompts.Prompting.Model;
using Prompts.Service.PromptService;
using Prompts.Service.ReportExecution;

namespace Prompts.Prompting.ViewModels.Implementation
{
    public class RecursiveChildTreeNodeService : IChildTreeNodeService
    {
        private readonly ITreeNodeCollectionBuilder _treeNodeCollectionBuilder;
        private readonly IChildPromptLevelServiceClient _childPromptLevelService;

        public RecursiveChildTreeNodeService(
            IChildPromptLevelServiceClient childPromptLevelService, 
            ITreeNodeCollectionBuilder treeNodeCollectionBuilder)
        {
            _childPromptLevelService = childPromptLevelService;
            _treeNodeCollectionBuilder = treeNodeCollectionBuilder;
        }

        public void GetChildrenFor(
            string promptName, 
            string parameterName, 
            ITreeNode treeNode, 
            Action<ObservableCollection<ITreeNode>> result)
        {
            _childPromptLevelService.GetChildrenForRecursive(
                promptName
                , parameterName
                , new ParameterValue {Name = parameterName, Value = treeNode.Value}
                , r => OnGetChildrenCompleted(r, promptName, treeNode, result)
                , errorMessage => ErrorGettingChildTreeNodes(this, new ServiceErrorEventArgs(errorMessage)));
        }

        public event EventHandler<ServiceErrorEventArgs> ErrorGettingChildTreeNodes;

        private void OnGetChildrenCompleted(
            PromptLevel response,
            string promptName,
            ITreeNode parentTreeNode,
            Action<ObservableCollection<ITreeNode>> callback)
        {
            var treeNodes = _treeNodeCollectionBuilder.BuildRegularNodesFrom(
                promptName
                , response.ParameterName
                , response.AvailableItems
                , parentTreeNode
                , response.HasChildLevel);

            callback(treeNodes);
        }
    }
}