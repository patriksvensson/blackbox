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
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Collections
{
	[TestFixture]
	public class LruCacheTests
	{
		[Test]
		public void LruCache_Put()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.AreEqual(2, cache.Count);
		}

		[Test]
		public void LruCache_Contains()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.IsTrue(cache.Contains(1));
			Assert.IsTrue(cache.Contains(2));
			Assert.IsFalse(cache.Contains(3));
		}

		[Test]
		public void LruCache_Clear()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.AreEqual(2, cache.Count);
			cache.Clear();
			Assert.AreEqual(0, cache.Count);
		}

		[Test]
		public void LruCache_Get()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.IsNotNull(cache.Get(1));
			Assert.IsNotNull(cache.Get(2));
		}

		[Test]
		public void LruCache_LeastUsedObjectIsRemovedWhenCapacityIsReached()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.IsNotNull(cache.Get(1));
			Assert.IsNotNull(cache.Get(2));
			Assert.AreEqual(2, cache.Count);
			cache.Put(3, "Three");
			Assert.AreEqual(2, cache.Count);
			Assert.IsNull(cache.Get(1));
			Assert.IsNotNull(cache.Get(2));
			Assert.IsNotNull(cache.Get(3));
			cache.Put(4, "Four");
			Assert.AreEqual(2, cache.Count);
			Assert.IsNull(cache.Get(2));
			Assert.IsNotNull(cache.Get(3));
			Assert.IsNotNull(cache.Get(4));
		}

		[Test]
		public void LruCache_MakeSureItemRemovedEventIsCalledWhenClearingList()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			List<string> removedList = new List<string>();
			cache.ItemRemoved += (o, s) => { removedList.Add(s.Item); };
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.AreEqual(0, removedList.Count);
			cache.Clear();
			Assert.AreEqual(2, removedList.Count);
			Assert.AreEqual("One", removedList[0]);
			Assert.AreEqual("Two", removedList[1]);
		}

		[Test]
		public void LruCache_MakeSureItemRemovedEventIsCalledWhenCapacityIsReached()
		{
			LruCache<int, string> cache = new LruCache<int, string>(1);
			List<string> removedList = new List<string>();
			cache.ItemRemoved += (o, s) => { removedList.Add(s.Item); };
			cache.Put(1, "One");			
			Assert.AreEqual(0, removedList.Count);
			cache.Put(2, "Two");
			Assert.AreEqual(1, removedList.Count);
			Assert.AreEqual("One", removedList[0]);
		}

		[Test]
		public void LruCache_MakeSureItemRemovedEventIsCalledWhenCacheIsDisposed()
		{
			LruCache<int, string> cache = new LruCache<int, string>(2);
			List<string> removedList = new List<string>();
			cache.ItemRemoved += (o, s) => { removedList.Add(s.Item); };
			cache.Put(1, "One");
			cache.Put(2, "Two");
			Assert.AreEqual(0, removedList.Count);
			cache.Dispose();
			Assert.AreEqual(2, removedList.Count);
			Assert.AreEqual("One", removedList[0]);
			Assert.AreEqual("Two", removedList[1]);
		}
	}
}
