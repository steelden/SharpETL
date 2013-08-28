using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpETL.Utility;
using Moq;
using SharpETL.Configuration;
using SharpETL.Components;
using SharpETL.Actions;

namespace SharpETL.Test
{
    [TestClass]
    public class ActionGraphTest
    {
        private IList<IAction> GetMockedActions(int count)
        {
            IList<IAction> result = new List<IAction>();
            for (int i = 0; i < count; ++i) {
                Mock<IAction> maction = new Mock<IAction>();
                maction.Setup(x => x.Id).Returns("action" + (i+1).ToString());
                result.Add(maction.Object);
            }
            return result;
        }

        private ActionGraph GetMockedGraph(IEnumerable<IAction> actions, IEnumerable<ILink> links)
        {
            var mconfig = new Mock<IEngineConfiguration>();
            mconfig.Setup(x => x.Actions).Returns(actions);
            mconfig.Setup(x => x.Links).Returns(links);
            return new ActionGraph(mconfig.Object);
        }

        private void GetActionsBinaryHeapTreeImpl(IAction[] actions, IList<ILink> result, int startIndex)
        {
            if (startIndex-1 < actions.Length) {
                IAction current = actions[startIndex-1];
                for (int i = startIndex * 2; i <= startIndex * 2 + 1; ++i) {
                    if (i-1 < actions.Length) {
                        result.Add(new DataLink(current, actions[i-1]));
                        GetActionsBinaryHeapTreeImpl(actions, result, i);
                    }
                }
            }
        }

        private IList<ILink> GetActionsBinaryHeapTree(IAction[] actions)
        {
            IList<ILink> result = new List<ILink>();
            GetActionsBinaryHeapTreeImpl(actions, result, 1);
            return result;
        }

        [TestMethod]
        public void ActionGraph_GetActionsInDependencyOrder_zero_actions_zero_links()
        {
            var actions = new List<IAction>();
            var links = new List<ILink>();
            var graph = GetMockedGraph(actions, links);
            Assert.AreEqual(0, graph.GetActionsInDependencyOrder().Count());
        }

        [TestMethod]
        public void ActionGraph_GetActionsInDependencyOrder_one_action_zero_links()
        {
            var actions = GetMockedActions(1);
            var links = new List<ILink>();
            var graph = GetMockedGraph(actions, links);
            Assert.AreEqual(1, graph.GetActionsInDependencyOrder().Count());
        }

        [TestMethod]
        public void ActionGraph_GetActionsInDependencyOrder_two_actions_zero_links()
        {
            var actions = GetMockedActions(2);
            var links = new List<ILink>();
            var graph = GetMockedGraph(actions, links);
            Assert.AreEqual(2, graph.GetActionsInDependencyOrder().Count());
        }

        [TestMethod]
        public void ActionGraph_GetActionsInDependencyOrder_two_actions_one_link()
        {
            var actions = GetMockedActions(2).ToArray();
            var links = new List<ILink>() { new DataLink(actions[0], actions[1]) };
            var graph = GetMockedGraph(actions, links);
            Assert.AreEqual(2, graph.GetActionsInDependencyOrder().Count());
        }

        [TestMethod]
        public void ActionGraph_GetActionsInDependencyOrder_many_actions_many_links()
        {
            int actionsCount = 20;
            var actions = GetMockedActions(actionsCount).ToArray();
            var links = GetActionsBinaryHeapTree(actions);
            var graph = GetMockedGraph(actions, links);
            var inOrder = graph.GetActionsInDependencyOrder();
            Assert.AreEqual(actionsCount, inOrder.Count());
        }
    }
}
