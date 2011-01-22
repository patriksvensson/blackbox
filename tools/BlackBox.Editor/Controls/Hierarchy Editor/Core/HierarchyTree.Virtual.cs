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
    public partial class HierarchyTree
    {
        #region Constants

        private const string VirtualNodeKey = "{75A21630-0CF5-4A7D-BD49-34129FE72CF8}";

        #endregion

        /// <summary>
        /// Determines whether a hiearchy node has a virtual node.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns>
        /// 	<c>true</c> if [has virtual node] [the specified root]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasVirtualNode(HierarchyNode root)
        {
            // No root?
            if (root == null)
            {
                return false;
            }

            foreach (TreeNode node in root.TreeNode.Nodes)
            {
                if (node.Text == HierarchyTree.VirtualNodeKey)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the virtual node for a hiearchy node.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        public TreeNode GetVirtualNode(HierarchyNode root)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a virtual node to a hiearchy node.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="check">if set to <c>true</c> [check].</param>
        public void AddVirtualNode(HierarchyNode root, bool check)
        {
            if (check) {
                if (this.HasVirtualNode(root)) {
                    return;
                }
            }

            // Add the virtual node.
            root.TreeNode.Nodes.Add(HierarchyTree.VirtualNodeKey);
        }

        /// <summary>
        /// Removes the virtual node from a hiearchy node.
        /// </summary>
        /// <param name="root">The root.</param>
        public void RemoveVirtualNode(HierarchyNode root)
        {
            foreach (TreeNode node in root.TreeNode.Nodes) {
                if (node.Text.Equals(HierarchyTree.VirtualNodeKey)) {
                    node.Remove();
                    return;
                }
            }
        }
    }
}
