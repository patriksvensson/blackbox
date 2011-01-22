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
using System.Drawing;

namespace BlackBox.Editor
{
    /// <summary>
    /// Represents an action that will be available in the
    /// context menu for a hierarchy node.
    /// </summary>
    public class HierarchyTask
    {
        #region Private Fields

        private readonly HierarchyNode m_owner;
        private readonly Action<HierarchyNode> m_action;
        private string m_title;
        private bool m_enabled;
        private bool m_visible;
        private Image m_image;
        private int m_sortOrder;
        private int m_group;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public HierarchyNode Owner
        {
            get { return m_owner; }
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public Action<HierarchyNode> Action
        {
            get { return m_action; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchyTask"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="HierarchyTask"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image
        {
            get { return m_image; }
            set { m_image = value; }
        }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>The sort order.</value>
        public int SortOrder
        {
            get { return m_sortOrder; }
            set { m_sortOrder = value; }
        }

        /// <summary>
        /// Gets or sets the group.
        /// </summary>
        /// <value>The group.</value>
        public int Group
        {
            get { return m_group; }
            set { m_group = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyTask"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="action">The action.</param>
        public HierarchyTask(string title, HierarchyNode owner, Action<HierarchyNode> action)
        {
            m_owner = owner;
            m_action = action;
            m_title = title;
            m_enabled = true;
            m_visible = true;
            m_image = null;
            m_group = 0;
            m_sortOrder = 0;
        }

        #endregion
    }
}
