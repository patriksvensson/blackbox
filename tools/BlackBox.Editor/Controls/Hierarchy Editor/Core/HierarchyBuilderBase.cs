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
    public abstract class HierarchyBuilderBase
    {
        private readonly HierarchyTree m_tree;
        private readonly IList<HierarchyNodeProvider> m_providers;
        private readonly ImageListMapper<string> m_mapper;
        private HierarchyContext m_context;

        #region Properties

        /// <summary>
        /// Gets the tree.
        /// </summary>
        /// <value>The tree.</value>
        public HierarchyTree Tree
        {
            get { return m_tree; }
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public IList<HierarchyNodeProvider> Providers
        {
            get { return m_providers; }
        }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        public ImageListMapper<string> Mapper
        {
            get { return m_mapper; }
        }

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        public HierarchyContext Context
        {
            get { return m_context; }
            set { m_context = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyTreeBuilderBase"/> class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="providers">The providers.</param>
        protected HierarchyBuilderBase(HierarchyTree tree, IEnumerable<HierarchyNodeProvider> providers)
        {
            m_tree = tree;
            m_providers = new List<HierarchyNodeProvider>(providers);
            m_mapper = new ImageListMapper<string>();
            m_tree.ImageList = m_mapper.ImageList;
        }

        /// <summary>
        /// Initializes the hierarchy builder.
        /// </summary>
        public virtual void Initialize(HierarchyContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Updates the tree nodes physical parameters.
        /// </summary>
        /// <param name="root">The root.</param>
        protected void UpdateNodeAppearance(HierarchyNode root)
        {
            if (root == null || root.TreeNode == null) {
                return;
            }

            root.TreeNode.Text = root.Text;
            root.TreeNode.ImageIndex = m_mapper.GetImageIndex(root.ImageKey);
            root.TreeNode.SelectedImageIndex = root.TreeNode.ImageIndex;
        }

        /// <summary>
        /// Gets the tree node collection.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns></returns>
        protected virtual TreeNodeCollection GetTreeNodeCollection(HierarchyNode root)
        {
            // Get the tree nodes.
            TreeNodeCollection treeNodes = (root == null) ? m_tree.Nodes : null;

            // No tree node collection?
            if (treeNodes == null && root != null)
            {
                // The root have no tree node?
                if (root.TreeNode == null)
                {
                    // Try to find the tree node.
                    root.TreeNode = this.FindTreeNode(null, root);
                    if (root.TreeNode == null)
                    {
                        throw new InvalidOperationException("Could not retrieve tree node.");
                    }
                }

                // Set the tree nodes.
                treeNodes = root.TreeNode.Nodes;
            }

            // Return the collection.
            return treeNodes;
        }

        /// <summary>
        /// Finds the tree node.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        protected virtual TreeNode FindTreeNode(HierarchyNode root, HierarchyNode target)
        {
            // Get the tree nodes.
            TreeNodeCollection treeNodes = this.GetTreeNodeCollection(root);

            // Iterate through all tree nodes.
            foreach (TreeNode treeNode in treeNodes)
            {
                // Get the hierarchy node.
                HierarchyNode node = treeNode.Tag as HierarchyNode;
                if (node == null)
                {
                    continue;
                }

                // Found a key match?
                if (node.Key == target.Key)
                {
                    return treeNode;
                }
            }

            // The node could not be found.
            return null;
        }

        /// <summary>
        /// Creates the tree node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected virtual TreeNode CreateTreeNode(HierarchyNode node)
        {
            TreeNode treeNode = new TreeNode();
            treeNode.Tag = node;
            node.TreeNode = treeNode;
            return treeNode;
        }

        /// <summary>
        /// Removes missing nodes.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="root">The root.</param>
        /// <param name="children">The children.</param>
        protected virtual void RemoveMissingNodes(HierarchyNodeProvider provider, HierarchyNode root, IEnumerable<HierarchyNode> children)
        {
            // Get the tree nodes.
            TreeNodeCollection treeNodes = this.GetTreeNodeCollection(root);

            if (treeNodes == null && root != null)
            {
                // No roots?
                if (root.TreeNode == null) {
                    // Find the root.
                    root.TreeNode = this.FindTreeNode(null, root);
                }

                // Get the tree nodes.
                treeNodes = root.TreeNode.Nodes;
            }

            // We found the node, now check if there's a node
            // that belongs to the same provider but no longer exist.            
            for (int index = treeNodes.Count - 1; index >= 0; index--)
            {
                // Get the tree node.
                TreeNode treeNode = treeNodes[index];
                if (treeNode.Tag == null)
                {
                    continue;
                }

                // Same hierarhcy provider?
                if (((HierarchyNode)treeNode.Tag).Provider == provider)
                {
                    bool found = false;
                    foreach (var childNode in children)
                    {
                        // Same key?
                        if (childNode.Key == ((HierarchyNode)treeNode.Tag).Key)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        // Remove the node.
                        ((HierarchyNode)treeNode.Tag).TreeNode.Remove();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified root has children.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns>
        /// 	<c>true</c> if the specified root has children; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool HasChildren(HierarchyContext context, HierarchyNode root)
        {
            foreach (HierarchyNodeProvider provider in m_providers)
            {
                if (provider.HasChildren(context, root))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
