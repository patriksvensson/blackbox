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
    public static class HierarchyNodeExtensions
    {
        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HierarchyNode GetParent(this HierarchyNode node)
        {
            // Make sure we have a valid node.
            if (node == null || node.TreeNode == null) {
                return null;
            }

            // Get the parent tree node.
            TreeNode parentTreeNode = node.TreeNode.Parent;
            if (parentTreeNode == null) {
                return null;
            }

            // Return the parent tree node's hierarchy node.
            return parentTreeNode.Tag as HierarchyNode;
        }

        /// <summary>
        /// Refreshes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static HierarchyNode Refresh(this HierarchyNode node)
        {
            if (node == null) {
                return null;
            }

            // Get the builder and the context.
            HierarchyBuilder builder = node.Provider.Builder;
            if (builder == null) {
                throw new InvalidOperationException("Could not resolve builder. Orphaned node?");
            }

            // Build the node.
            return builder.Build(node);   
        }

        public static HierarchyNode RefreshParent(this HierarchyNode node)
        {
            if (node == null) {
                return null;
            }

            // Get the parent node.
            HierarchyNode parentNode = node.GetParent();
            if (parentNode == null)
            {
                throw new InvalidOperationException("Could not resolve parent node.");
            }

            // Refresg the node.
            return parentNode.Refresh();
        }
    }
}
