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
using BlackBox.Editor.Properties;

namespace BlackBox.Editor
{
    public class HierarchyBuilder : HierarchyBuilderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyBuilder"/> class.
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <param name="providers">The providers.</param>
        public HierarchyBuilder(HierarchyTree tree, IEnumerable<HierarchyNodeProvider> providers)
            : base(tree, providers)
        {
        }

        /// <summary>
        /// Initializes the hierarchy builder.
        /// </summary>
        public override void Initialize(HierarchyContext context)
        {
            base.Initialize(context);

            // Iterate through all the providers.
            foreach (HierarchyNodeProvider provider in this.Providers)
            {
                // Set the provider's builder (this).
                provider.Builder = this;

                // Iterate through all the providers mappings.
                foreach (ImageMapping imageMapping in provider.GetImages())
                {
                    // Add the mapping to the mapper.
                    this.Mapper.AddImage(imageMapping.Key, imageMapping.Image);
                }
            }

            // Set the default image.
            this.Mapper.SetDefaultKey(typeof(HierarchyNode).FullName, Resources.Folder);
        }

        /// <summary>
        /// Builds the whole tree view.
        /// </summary>
        public void Build()
        {
            this.Build(null);
        }

        /// <summary>
        /// Builds the tree view starting at the specified root.
        /// </summary>
        /// <param name="root">The root.</param>
        public HierarchyNode Build(HierarchyNode root)
        {
            // Iterate through all the providers.
            foreach (HierarchyNodeProvider provider in this.Providers)
            {
                // Update the root node.
                this.UpdateNodeAppearance(root);

                // Get all the provider's children for the current node.
                // If no children; continue
                var children = provider.GetChildren(this.Context, root).ToList();
                if (children == null || children.Count() == 0) 
                {
                    this.RemoveMissingNodes(provider, root, children);
                    continue;
                }

                // Remove missing nodes.
                // With missing nodes, we mean nodes that exists under the root node,
                // but wasn't retrieved by the provider.
                this.RemoveMissingNodes(provider, root, children);

                // Iterate all child nodes.
                foreach (HierarchyNode child in children)
                {
                    // Try to find the child tree node.
                    TreeNode childTreeNode = this.FindTreeNode(root, child);
                    bool foundChildTreeNode = childTreeNode != null;
                    if (childTreeNode == null)
                    {
                        // Create the node.
                        childTreeNode = this.CreateTreeNode(child);
                    }

                    // Update the tree node's tag.
                    childTreeNode.Tag = child;
                    child.TreeNode = childTreeNode;

                    // Update the child tree node's physical parameters.
                    this.UpdateNodeAppearance(child);

                    // Does the node support children?
                    if (child.SupportChildren)
                    {
                        // Root have placeholder node?
                        if (this.Tree.HasVirtualNode(root))
                        {
                            // Remove the placeholder node.
                            this.Tree.RemoveVirtualNode(root);

                            // Child have children of it's own?
                            if (this.HasChildren(this.Context, child))
                            {
                                // Add placeholder node.
                                this.Tree.AddVirtualNode(child, false);
                            }
                        }
                        else
                        {
                            // Does the child tree node have children?
                            if (childTreeNode.Nodes.Count > 0)
                            {
                                // Is the tree node expanded?
                                if (childTreeNode.IsExpanded)
                                {
                                    // Refresh the hierarchy child nodes recursivly.
                                    this.Build(child);
                                }
                            }
                            else
                            {
                                // Child have children of it's own?
                                if (this.HasChildren(this.Context, child))
                                {
                                    // Add placeholder node.
                                    this.Tree.AddVirtualNode(child, false);
                                }
                            }
                        }
                    }

                    // Did we not find the tree node before?
                    if (!foundChildTreeNode)
                    {
                        // Add the child tree node to the root tree node collection. 
                        this.GetTreeNodeCollection(root).Add(childTreeNode);
                    }

                    // Should the child node auto expand?
                    if (child.AutoExpand)
                    {
                        // Is the child tree node not expanded?
                        if (!child.TreeNode.IsExpanded)
                        {
                            // Refresh the child node.
                            this.Build(child);

                            // Expand the tree node.
                            child.TreeNode.Expand();
                        }
                    }
                }
            }

            // Return the node.
            return root;
        }
    }
}
