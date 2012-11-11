using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Prompts.Prompting.ViewModels;
using Prompts.Prompting.ViewModels.Implementation;
using Prompts.Service.ReportExecution;
using Test.Prompts.Infrastructure;

namespace Test.Prompts.Prompting.ViewModels.Implementation
{
    [TestClass]
    public class TreeNodeTest
    {
        [TestMethod]
        public void ItRaisesAnEventWhenIsSelectedIsSetToTrue()
        {
            var numberOfEvents = 0;

            var treeNode = new TreeNodeForTest();
            treeNode.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "IsSelected")
                    {
                        numberOfEvents++;
                    }
                };

            treeNode.IsSelected = true;
            Assert.AreEqual(1, numberOfEvents);
        }

        [TestMethod]
        public void ItRaisesAnEventWhenIsSelectedIsSetToFalse()
        {
            var numberOfEvents = 0;

            var treeNode = new TreeNodeForTest();
            treeNode.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "IsSelected")
                {
                    numberOfEvents++;
                }
            };

            treeNode.IsSelected = false;
            Assert.AreEqual(1, numberOfEvents);
        }

        private class TreeNodeForTest : TreeNode
        {
            public TreeNodeForTest() : base(
                "Prompt Name",
                "Parameter Name",
                A.ValidValue().Build(),
                new ObservableCollection<ITreeNode>(),
                Mock.Of<ITreeNode>(),
                false,
                false)
            {
            }

            public override bool IsExpanded
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }
    }
}
