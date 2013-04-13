using Prompts.Prompting.Construction;
using Prompts.Prompting.Construction.Implementation;
using Prompts.Prompting.Model;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;

namespace Prompts
{
    public class RecursiveTreeContainer
    {
        public IPromptBuilder Create()
        {
            return new ShoppingCartBuilder<ITreeNode>(
                CreateRootTreeNodeProvider(),
                CreateMultiSelectTreeProvider());
        }

        protected virtual IShoppingCartProvider<ITreeNode> CreateMultiSelectTreeProvider()
        {
            return new MultiSelectTreeProvider();
        }

        private static IPromptItemProvider<ITreeNode> CreateRootTreeNodeProvider()
        {
            var treeNodeBuilder = new TreeNodeBuilder();
            var treeNodeCollectionBuilder = CreateTreeNodeCollectionBuilder(treeNodeBuilder);
            treeNodeBuilder.ChildTreeNodeService = CreateChildTreeNodeService(treeNodeCollectionBuilder);
            return new RootTreeNodeProvider(CreateChildTreeNodeService(treeNodeCollectionBuilder));
        }

        private static IChildTreeNodeService CreateChildTreeNodeService(
            ITreeNodeCollectionBuilder treeNodeCollectionBuilder)
        {
            return new RecursiveChildTreeNodeService(
                ServiceInjector.Inject<IChildPromptLevelServiceClient>()
                , treeNodeCollectionBuilder);
        }

        private static ITreeNodeCollectionBuilder CreateTreeNodeCollectionBuilder(ITreeNodeBuilder treeNodeBuilder)
        {
            return new TreeNodeCollectionBuilder(treeNodeBuilder);
        }
    }
}