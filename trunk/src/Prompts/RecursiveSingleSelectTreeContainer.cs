using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;

namespace Prompts
{
    public class RecursiveSingleSelectTreeContainer
    {
        public IPromptBuilder Create()
        {
            return new SingleSelectPromptBuilder<ITreeNode>(
                InjectRootTreeNodeProvider()
                , InjectSingleSelectTreeProvider());
        }

        protected virtual ISingleSelectPromptProvider<ITreeNode> CreateMultiSelectTreeProvider()
        {
            return new SingleSelectTreeProvider();
        }

        private static ISingleSelectPromptProvider<ITreeNode> InjectSingleSelectTreeProvider()
        {
            return new SingleSelectTreeProvider();
        }

        private static IPromptItemProvider<ITreeNode> InjectRootTreeNodeProvider()
        {
            var treeNodeBuilder = new TreeNodeBuilder();
            var treeNodeCollectionBuilder = InjectTreeNodeCollectionBuilder(treeNodeBuilder);
            treeNodeBuilder.ChildTreeNodeService = InjectChildTreeNodeService(treeNodeCollectionBuilder);
            return new RootTreeNodeProvider(InjectChildTreeNodeService(treeNodeCollectionBuilder));
        }

        private static IChildTreeNodeService InjectChildTreeNodeService(ITreeNodeCollectionBuilder treeNodeCollectionBuilder)
        {
            return new RecursiveChildTreeNodeService(
                ServiceInjector.Inject<IChildPromptLevelServiceClient>()
                , treeNodeCollectionBuilder);
        }

        private static ITreeNodeCollectionBuilder InjectTreeNodeCollectionBuilder(ITreeNodeBuilder treeNodeBuilder)
        {
            return new TreeNodeCollectionBuilder(treeNodeBuilder);
        }
    }
}