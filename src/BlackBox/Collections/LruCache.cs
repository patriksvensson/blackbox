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
using System.Collections;
using System.Collections.Generic;

namespace BlackBox
{
	internal sealed class LruCache<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, IDisposable
	{
		private readonly IndexedLinkedList<TKey> _keys;
		private readonly Dictionary<TKey, TValue> _lookup;
		private readonly int _capacity;
		private bool _disposed;

		#region Events

		public event EventHandler<EventArgs<TValue>> ItemRemoved = (s, e) => { };

		#endregion

		#region Properties

		internal int Count
		{
			get { return _keys.Count; }
		}

		#endregion

		#region Construction

		internal LruCache(int capacity)
		{
			_capacity = capacity;
			_keys = new IndexedLinkedList<TKey>();
			_lookup = new Dictionary<TKey, TValue>();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					this.Clear();
				}
				_disposed = true;
			}
		}

		#endregion

		internal bool Contains(TKey key)
		{
			return _lookup.ContainsKey(key);
		}

		internal TValue Get(TKey key)
		{
			if (this.Contains(key))
			{
				_keys.Remove(key);
				_keys.AddFirst(key);
				return _lookup[key];
			}
			return default(TValue);
		}

		internal void Put(TKey key, TValue value)
		{
			_keys.Remove(key);
			_keys.AddFirst(key);
			_lookup[key] = value;

			if (_keys.Count > _capacity)
			{
				// Get the value we're removing.
				TValue oldValue = _lookup[_keys.Last];

				// Remove the key and the item from the list.
				_lookup.Remove(_keys.Last);
				_keys.RemoveLast();

				// Let the owner of the cache decide what happens to the object.
				this.ItemRemoved(this, new EventArgs<TValue>(oldValue));
			}
		}

		internal void Clear()
		{
			foreach (var pair in _lookup)
			{
				// Let the owner of the cache decide what happens to the object.
				this.ItemRemoved(this, new EventArgs<TValue>(pair.Value));
			}

			_lookup.Clear();
			_keys.Clear();
		}

		#region IEnumerable<KeyValuePair<TKey,TValue>> Members

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (TKey key in _keys)
			{
				yield return new KeyValuePair<TKey, TValue>(key, _lookup[key]);
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
