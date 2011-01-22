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
    public abstract class HierarchyNode : IEquatable<HierarchyNode>
    {
        #region Private Fields

        private readonly HierarchyNodeProvider m_provider;
        private TreeNode m_treeNode;
		private readonly Guid _guid;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <value>The provider.</value>
        public HierarchyNodeProvider Provider
        {
            get { return m_provider; }
        }

        /// <summary>
        /// Gets or sets the tree node.
        /// </summary>
        /// <value>The tree node.</value>
        public TreeNode TreeNode
        {
            get { return m_treeNode; }
            set { m_treeNode = value; }
        }

        /// <summary>
        /// Gets the image key.
        /// </summary>
        /// <value>The image key.</value>
        public virtual string ImageKey
        {
            get { return this.GetType().FullName; }
        }

        /// <summary>
        /// Gets a value indicating whether this node support children.
        /// </summary>
        /// <value><c>true</c> if the node support children; otherwise, <c>false</c>.</value>
        public virtual bool SupportChildren
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the node should auto expand.
        /// </summary>
        /// <value><c>true</c> if the node should auto expand; otherwise, <c>false</c>.</value>
        public virtual bool AutoExpand
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public virtual string Key
		{
			get { return _guid.ToString(); }
		}

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>The text.</value>
        public abstract string Text { get; }

        /// <summary>
        /// Gets a value indicating whether this node can be remove.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this node can be remove; otherwise, <c>false</c>.
        /// </value>
        public virtual bool CanRemove
        {
            get { return false; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchyNode"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        protected HierarchyNode(HierarchyNodeProvider provider)
        {
            m_provider = provider;
			_guid = Guid.NewGuid();
        }

        #endregion

        #region IEquatable<HierarchyNode> Members

        /// <summary>
        /// Determines whether the specified <see cref="HierarchyNode"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        public bool Equals(HierarchyNode other)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (obj is HierarchyNode)
            {
                this.Equals((HierarchyNode)obj);
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        /// <summary>
        /// Gets the associated data object.
        /// </summary>
        /// <returns></returns>
        public virtual object GetDataObject()
        {
            return null;
        }

        /// <summary>
        /// Gets the tasks.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<HierarchyTask> GetTasks()
        {
            yield break;
        }

        /// <summary>
        /// Removes this node from the hierarchy.
        /// </summary>
        public virtual bool Remove()
        {
            return false;
        }
    }
}
