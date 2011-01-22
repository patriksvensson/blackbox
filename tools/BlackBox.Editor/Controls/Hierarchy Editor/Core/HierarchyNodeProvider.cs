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

namespace BlackBox.Editor
{
    public abstract class HierarchyNodeProvider
    {
        private HierarchyBuilder m_builder;

        /// <summary>
        /// Gets or sets the builder.
        /// </summary>
        /// <value>The builder.</value>
        public HierarchyBuilder Builder
        {
            get { return m_builder; }
            internal set { m_builder = value; }
        }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ImageMapping> GetImages()
        {
            yield break;
        }

        /// <summary>
        /// Gets the children of the specified <see cref="Mapper.Library.Hierarchy.HierarchyNode"/>.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual IEnumerable<HierarchyNode> GetChildren(HierarchyContext context, HierarchyNode node)
        {
            yield break;
        }

        /// <summary>
        /// Determines whether the specified node has children.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node has children; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasChildren(HierarchyContext context, HierarchyNode node)
        {
            return false;
        }

        /// <summary>
        /// Gets the tasks for a hierarchy node.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<HierarchyTask> GetTasks(HierarchyContext context, HierarchyNode node)
        {
            yield break;
        }
    }
}
