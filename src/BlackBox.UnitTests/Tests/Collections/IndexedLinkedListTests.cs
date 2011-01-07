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
    public class IndexedLinkedListTests
    {
        [Test]
        public void IndexedLinkedList_AddFirst()
        {
            IndexedLinkedList<string> list = new IndexedLinkedList<string>();
            list.AddFirst("3");
            list.AddFirst("2");
            list.AddFirst("1");
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("1", list.First);
        }

        [Test]
        public void IndexedLinkedList_AddLast()
        {
            IndexedLinkedList<string> list = new IndexedLinkedList<string>();
            list.AddLast("3");
            list.AddLast("2");
            list.AddLast("1");
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("1", list.Last);
        }

        [Test]
        public void IndexedLinkedList_RemoveLast()
        {
            IndexedLinkedList<string> list = new IndexedLinkedList<string>();
            list.AddLast("3");
            list.AddLast("2");
            list.AddLast("1");
            Assert.AreEqual("1", list.Last);
            list.RemoveLast();
            Assert.AreEqual("2", list.Last);
        }

        [Test]
        public void IndexedLinkedList_RemoveFirst()
        {
            IndexedLinkedList<string> list = new IndexedLinkedList<string>();
            list.AddFirst("3");
            list.AddFirst("2");
            list.AddFirst("1");
            Assert.AreEqual("1", list.First);
            list.RemoveFirst();
            Assert.AreEqual("2", list.First);
        }

        [Test]
        public void IndexedLinkedList_Remove()
        {
            IndexedLinkedList<string> list = new IndexedLinkedList<string>();
            list.AddFirst("3");
            list.AddFirst("2");
            list.AddFirst("1");
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("1", list.First);
            list.Remove("1");
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual("2", list.First);
            list.Remove("3");
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("2", list.First);
        }

        [Test]
        public void IndexedLinkedList_Clear()
        {
            IndexedLinkedList<string> list = new IndexedLinkedList<string>();
            list.AddFirst("3");
            list.AddFirst("2");
            list.AddFirst("1");
            Assert.AreEqual(3, list.Count);
            list.Clear();
            Assert.AreEqual(0, list.Count);
            Assert.IsNull(list.First);
            Assert.IsNull(list.Last);
        }
    }
}
