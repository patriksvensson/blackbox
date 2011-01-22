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
    public class LogFilterNodeProvider : HierarchyNodeProvider
	{
		public override IEnumerable<ImageMapping> GetImages()
		{
			yield return new ImageMapping(typeof(LogFilterNode).FullName, Resources.Filter);
		}

        public override IEnumerable<HierarchyNode> GetChildren(HierarchyContext context, HierarchyNode node)
		{
			if (node == null)
			{
				yield return new LogFilterCollectionNode(this, context.Configuration);
			}
			else if (node is LogFilterCollectionNode)
			{
				foreach (LogFilter filter in context.Configuration.Filters)
				{
					yield return new LogFilterNode(this, node, filter);
				}
			}
			else if (node is LogSinkNode)
			{
				foreach (LogFilter filter in ((LogSinkNode)node).Sink.Filters)
				{
					yield return new LogFilterNode(this, node, filter);
				}
			}
            else if (node is LogSinkProxyNode)
            {
                foreach (LogFilter filter in ((LogSinkProxyNode)node).Proxy.Filters)
                {
                    yield return new LogFilterNode(this, node, filter);
                }
            }
		}

        public override bool HasChildren(HierarchyContext context, HierarchyNode node)
		{
			if (node == null)
			{
				return true;
			}
			else if (node is LogFilterCollectionNode)
			{
				return context.Configuration.Filters.Count > 0;
			}
			else if (node is LogSinkNode)
			{
				return ((LogSinkNode)node).Sink.Filters.Count > 0;
			}
            else if (node is LogSinkProxyNode)
            {
                return ((LogSinkProxyNode)node).Proxy.Filters.Count > 0;
            }
			else
			{
				return false;
			}
		}

        public override IEnumerable<HierarchyTask> GetTasks(HierarchyContext context, HierarchyNode node)
        {
            if (node is LogFilterCollectionNode)
            {
                yield return HierarchyTaskFactory.AddFilterTask(node);
            }
            else if (node is LogFilterNode)
            {
                yield return HierarchyTaskFactory.RemoveFilterTask(node);
            }
        }
	}
}
