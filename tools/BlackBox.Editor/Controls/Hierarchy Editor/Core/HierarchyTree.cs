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
    public partial class HierarchyTree : TreeView
    {
        #region Events

        /// <summary>
        /// Occurs when the hierarchy context have changed.
        /// </summary>
        public event EventHandler HierarchyContextChanged = (sender, args) => { };

        /// <summary>
        /// Occurs when a hierarchy node is selected.
        /// </summary>
        public event EventHandler<HierarchyNodeEventArgs> HierarchyNodeSelected = (sender, args) => { };

        /// <summary>
        /// Occurs when the tree becomes dirty.
        /// </summary>
        public event EventHandler Dirty = (sender, args) => { };

        #endregion

        #region Private Fields

        private HierarchyBuilder m_builder;
        private bool m_initialized;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchyTree"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public bool Initialized
        {
            get { return m_initialized; }
            private set { m_initialized = value; }
        }

        /// <summary>
        /// Gets the builder.
        /// </summary>
        /// <value>The builder.</value>
        public HierarchyBuilder Builder
        {
            get { return m_builder; }
        }

        #endregion

        #region Tree Events

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.BeforeExpand"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewCancelEventArgs"/> that contains the event data.</param>
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            // Get the hierarchy node.
            HierarchyNode node = e.Node.Tag as HierarchyNode;
            if (node == null) {
                throw new InvalidOperationException("Could not resolve hierarchy node from tree node.");
            }

            // Do we have a placeholder node?
            if (this.HasVirtualNode(node)) {
                // Rebuild the node.
                this.Build(node);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterSelect"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"/> that contains the event data.</param>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse) 
            {
                if (e.Node != null && e.Node.Tag is HierarchyNode) 
                {
                    this.SelectedNode = e.Node;
                    this.HierarchyNodeSelected(this, new HierarchyNodeEventArgs(e.Node.Tag as HierarchyNode));
                }
            }

            base.OnAfterSelect(e);
        }

        #endregion

        /// <summary>
        /// Initializes the tree with the specified providers.
        /// </summary>
        /// <param name="providers">The providers.</param>
        public void Initialize(HierarchyContext context, IEnumerable<HierarchyNodeProvider> providers)
        {
            // Initialize the tree.
            this.ShowRootLines = true;
            this.ShowLines = true;
            this.ShowPlusMinus = true;
            this.ShowNodeToolTips = true;
            this.HideSelection = false;

            // Clear the tree of nodes.
            this.Nodes.Clear();

            // Create the builder.
            m_builder = new HierarchyBuilder(this, providers);
            m_builder.Initialize(context);

            // We're now initialized.
            m_initialized = true;
        }

        /// <summary>
        /// Raises the dirty event.
        /// </summary>
        public void RaiseDirty()
        {
            this.Dirty(this, EventArgs.Empty);
        }

        /// <summary>
        /// Builds this tree.
        /// </summary>
        public void Build()
        {
            if (!this.Initialized) {
                return;
            }

            // Build the tree.
            m_builder.Build();
        }

        /// <summary>
        /// Builds the specified node.
        /// </summary>
        /// <param name="root">The node.</param>
        public void Build(HierarchyNode root)
        {
            if (!this.Initialized) {
                return;
            }

            // Build the tree starting at the specified root.
            m_builder.Build(root);
        }
    }
}
