using System;
using System.Collections.ObjectModel;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;

namespace Test.Prompts.Infrastructure.Fakes
{
    internal class FakeChildTreeNodeService
    {
        private readonly Mock<IChildTreeNodeService> _childTreeNodeService;
        private Action<ObservableCollection<ITreeNode>> _callBack;

        public FakeChildTreeNodeService()
        {
            _childTreeNodeService = new Mock<IChildTreeNodeService>();
        }

        public void SetupGetChildren(string promptName, string parameterName, ITreeNode asynchronousTreeNode)
        {
            var setup = _childTreeNodeService
                .Setup(
                    s => s.GetChildrenFor(
                        promptName
                        , parameterName
                        , asynchronousTreeNode
                        , It.IsAny<Action<ObservableCollection<ITreeNode>>>()));

            setup.Callback(
                (string p, string pn, ITreeNode tn, Action<ObservableCollection<ITreeNode>> c)
                    => _callBack = c);
        }

        public void ExecuteGetChildrenCallback(ObservableCollection<ITreeNode> treeNodes)
        {
            _callBack(treeNodes);
        }

        public void AssertNumberOfGetChildren(
            string promptName
            , string parameterName
            , ITreeNode treeNode
            , Times times)
        {
            _childTreeNodeService.Verify(
                s => s.GetChildrenFor(
                    promptName
                    , parameterName
                    , treeNode
                    , It.IsAny<Action<ObservableCollection<ITreeNode>>>()), times);
        }

        public IChildTreeNodeService Object
        {
            get { return _childTreeNodeService.Object; }
        }

        public void RaiseErrorGettingChildTreeNodes(string errorMessage)
        {
            _childTreeNodeService.Raise(
                s => s.ErrorGettingChildTreeNodes += null, 
                new ServiceErrorEventArgs(errorMessage));
        }
    }
}
