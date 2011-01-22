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
using System.Windows.Forms;

namespace BlackBox.Editor
{
	public static class HierarchyTaskMethods
	{
        public static void AddLogSink(HierarchyNode node)
		{
			AddSinkDialog dialog = new AddSinkDialog();
			DialogResult result = dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				Type sinkType = dialog.SelectedType;
				if (sinkType != null)
				{
					// Create the sink.
					LogSink sink = Activator.CreateInstance(sinkType, sinkType.IsPublic) as LogSink;

					#region Add the sink to the configuration
					if (node is LogSinkCollectionNode)
					{
						// Add the node to the root.
						((LogSinkCollectionNode)node).Configuration.Sinks.Add(sink);
                        node.Provider.Builder.Tree.RaiseDirty();
					}
					else if (node is LogSinkProxyNode)
					{
						// Add the node to the proxy.
						((LogSinkProxyNode)node).Proxy.Sinks.Add(sink);
                        node.Provider.Builder.Tree.RaiseDirty();
					}
					#endregion

					// Rebuild the node (so the new sink will be added).
					node.Provider.Builder.Build(node);
				}
			}
		}

        public static void AddFilter(HierarchyNode node)
        {
            AddFilterDialog dialog = new AddFilterDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                Type filterType = dialog.SelectedType;
                if (filterType != null)
                {
                    // Create the filter.
                    LogFilter filter = Activator.CreateInstance(filterType, filterType.IsPublic) as LogFilter;

                    #region Add the filter to the configuration
                    if (node is LogFilterCollectionNode)
                    {
                        // Add the filter to the root.
                        ((LogFilterCollectionNode)node).Configuration.Filters.Add(filter);
                        node.Provider.Builder.Tree.RaiseDirty();
                    }
                    else if (node is LogSinkNode)
                    {
                        // Add the filter to the proxy.
                        ((LogSinkNode)node).Sink.Filters.Add(filter);
                        node.Provider.Builder.Tree.RaiseDirty();
                    }
                    else if (node is LogSinkProxyNode)
                    {
                        // Add the filter to the proxy.
                        ((LogSinkProxyNode)node).Proxy.Filters.Add(filter);
                        node.Provider.Builder.Tree.RaiseDirty();
                    }
                    #endregion

                    // Rebuild the node (so the new filter will be added).
                    node.Refresh();
                }
            }
        }

        public static void Remove(HierarchyNode node)
        {
            if (node.CanRemove)
            {
                if (node.Remove())
                {
                    node.Provider.Builder.Tree.RaiseDirty();
                }
            }
        }
	}
}
