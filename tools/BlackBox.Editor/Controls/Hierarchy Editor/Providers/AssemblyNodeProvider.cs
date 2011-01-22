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
using System.Reflection;

namespace BlackBox.Editor
{
    public class AssemblyNodeProvider : HierarchyNodeProvider
	{
        public override IEnumerable<ImageMapping> GetImages()
        {
            yield return new ImageMapping(typeof(AssemblyNode).FullName, Resources.Assembly);
        }

        public override IEnumerable<HierarchyNode> GetChildren(HierarchyContext context, HierarchyNode node)
		{
			// Currently at root level?
			if (node == null)
			{
				yield return new AssemblyCollectionNode(this, context.Configuration);
			}
			// Log sink collection?
			else if (node is AssemblyCollectionNode)
			{
				foreach (Assembly assembly in context.Configuration.Assemblies)
				{
                    yield return new AssemblyNode(this, node, assembly);
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
            else if (node is AssemblyCollectionNode)
            {
                return context.Configuration.Assemblies.Count > 0;
            }
            else
            {
                return false;
            }
		}

        public override IEnumerable<HierarchyTask> GetTasks(HierarchyContext context, HierarchyNode node)
        {
            if (node is AssemblyCollectionNode)
            {
                yield return HierarchyTaskFactory.AddAssemblyTask(node);
            }
            else if (node is AssemblyNode)
            {
                yield return HierarchyTaskFactory.RemoveAssemblyTask(node);
            }
        }
	}
}
