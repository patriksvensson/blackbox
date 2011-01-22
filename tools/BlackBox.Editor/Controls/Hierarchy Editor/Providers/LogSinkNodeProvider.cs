//
// Copyright 2011 Patrik Svensson
//
// This file is part of BlackBox.
//
// BlackBox is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// BlackBox is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser Public License for more details.
//
// You should have received a copy of the GNU Lesser Public License
// along with BlackBox. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlackBox.Editor.Properties;

namespace BlackBox.Editor
{
	public class LogSinkNodeProvider : HierarchyNodeProvider
	{
		public override IEnumerable<ImageMapping> GetImages()
		{
			yield return new ImageMapping(typeof(LogSinkNode).FullName, Resources.Sink);
			yield return new ImageMapping(typeof(LogSinkProxyNode).FullName, Resources.Proxy);
		}

        public override IEnumerable<HierarchyNode> GetChildren(HierarchyContext context, HierarchyNode node)
		{
			// Currently at root level?
			if (node == null)
			{
				yield return new LogSinkCollectionNode(this, context.Configuration);
			}
			// Log sink collection?
			else if (node is LogSinkCollectionNode)
			{
                // TODO: Find a better way of doing this. Should be that hard.
                List<LogSinkProxyNode> proxyNodes = new List<LogSinkProxyNode>();
                List<LogSinkNode> sinkNodes = new List<LogSinkNode>();

				foreach (LogSink sink in context.Configuration.Sinks)
				{
					if ((sink is LogSinkProxy))
					{
                        proxyNodes.Add(new LogSinkProxyNode(this, node, (LogSinkProxy)sink));
					}
					else
					{
                        sinkNodes.Add(new LogSinkNode(this, node, sink));
					}
				}

                foreach (LogSinkProxyNode proxyNode in proxyNodes)
                {
                    yield return proxyNode;
                }
                foreach (LogSinkNode sinkNode in sinkNodes)
                {
                    yield return sinkNode;
                }
			}
			// Reached a log sink proxy?
			else if (node is LogSinkProxyNode)
			{
                // TODO: Find a better way of doing this. Should be that hard.
                List<LogSinkProxyNode> proxyNodes = new List<LogSinkProxyNode>();
                List<LogSinkNode> sinkNodes = new List<LogSinkNode>();

				foreach (LogSink sink in ((LogSinkProxyNode)node).Proxy.Sinks)
				{
					if ((sink is LogSinkProxy))
					{
                        proxyNodes.Add(new LogSinkProxyNode(this, node, (LogSinkProxy)sink));
					}
					else
					{
                        sinkNodes.Add(new LogSinkNode(this, node, sink));
					}
				}

                foreach (LogSinkProxyNode proxyNode in proxyNodes)
                {
                    yield return proxyNode;
                }
                foreach (LogSinkNode sinkNode in sinkNodes)
                {
                    yield return sinkNode;
                }
			}
		}

        public override bool HasChildren(HierarchyContext context, HierarchyNode node)
		{
			// Currently at root level?
			if (node == null)
			{
				return true;
			}
			// Log sink collection?
			else if (node is LogSinkCollectionNode)
			{
				return context.Configuration.Sinks.Count > 0;
			}
			// Reached a log sink proxy?
			else if (node is LogSinkProxyNode)
			{
				return ((LogSinkProxyNode)node).Proxy.Sinks.Count > 0;
			}
			// The provider have no children for the node.
			else
			{
				return false;
			}
		}

        public override IEnumerable<HierarchyTask> GetTasks(HierarchyContext context, HierarchyNode node)
        {
            if (node is LogSinkCollectionNode)
            {
                yield return HierarchyTaskFactory.AddSinkTask(node);
            }
            else if (node is LogSinkProxyNode)
            {
                yield return HierarchyTaskFactory.AddSinkTask(node);
                yield return HierarchyTaskFactory.AddFilterTask(node);
                yield return HierarchyTaskFactory.RemoveSinkTask(node);
            }
            else if (node is LogSinkNode)
            {
                yield return HierarchyTaskFactory.AddFilterTask(node);
                yield return HierarchyTaskFactory.RemoveSinkTask(node);
            }
        }
	}
}
