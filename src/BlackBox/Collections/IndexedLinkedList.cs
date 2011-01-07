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
using System.Collections;

namespace BlackBox
{
    internal sealed class IndexedLinkedList<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _linkedList;
        private readonly Dictionary<T, LinkedListNode<T>> _index;

        #region Properties

        internal int Count
        {
            get { return _linkedList.Count; }
        }

        internal T First
        {
            get
            {
                return this.Count > 0
                    ? _linkedList.First.Value
                    : default(T);
            }
        }

        internal T Last
        {
            get
            {
                return this.Count > 0
                    ? _linkedList.Last.Value
                    : default(T);
            }
        }

        #endregion

        #region Construction

        internal IndexedLinkedList()
        {
            _linkedList = new LinkedList<T>();
            _index = new Dictionary<T, LinkedListNode<T>>();
        }

        #endregion

        internal void AddFirst(T item)
        {
            _index[item] = _linkedList.AddFirst(item);
        }

        internal void AddLast(T item)
        {
            _index[item] = _linkedList.AddLast(item);
        }

        internal void Remove(T item)
        {
            LinkedListNode<T> node;
            if (_index.TryGetValue(item, out node))
            {
                _linkedList.Remove(node);
                _index.Remove(item);
            }
        }

        internal void RemoveFirst()
        {
            _index.Remove(_linkedList.First.Value);
            _linkedList.RemoveFirst();
        }

        internal void RemoveLast()
        {
            _index.Remove(_linkedList.Last.Value);
            _linkedList.RemoveLast();
        }

        internal void Clear()
        {
            _linkedList.Clear();
            _index.Clear();
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
