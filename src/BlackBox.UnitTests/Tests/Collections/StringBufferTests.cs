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

using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Collections
{
	[TestFixture]
	public class StringBufferTests
	{
		[Test]
		public void StringBuffer_Create()
		{
			StringBuffer buffer = new StringBuffer("Hello World");
			Assert.AreEqual("Hello World", buffer.Content);
			Assert.AreEqual(11, buffer.Length);
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void StringBuffer_Peek()
		{
			StringBuffer buffer = new StringBuffer("Hello World");
			Assert.AreEqual(0, buffer.Position);
			Assert.AreEqual('H', (char)buffer.Peek());
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void StringBuffer_Read()
		{
			StringBuffer buffer = new StringBuffer("Hello World");
			Assert.AreEqual('H', (char)buffer.Read());
			Assert.AreEqual('e', (char)buffer.Read());
			Assert.AreEqual('l', (char)buffer.Read());
			Assert.AreEqual('l', (char)buffer.Read());
			Assert.AreEqual('o', (char)buffer.Read());
			Assert.AreEqual(' ', (char)buffer.Read());
			Assert.AreEqual('W', (char)buffer.Read());
			Assert.AreEqual('o', (char)buffer.Read());
			Assert.AreEqual('r', (char)buffer.Read());
			Assert.AreEqual('l', (char)buffer.Read());
			Assert.AreEqual('d', (char)buffer.Read());
			Assert.AreEqual(-1, buffer.Read());
		}

		[Test]
		public void StringBuffer_Rewind()
		{
			StringBuffer buffer = new StringBuffer("Hello World");
			Assert.IsTrue(buffer.Seek(6));
			Assert.AreEqual(6, buffer.Position);
			buffer.Rewind();
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void StringBuffer_Seek()
		{
			StringBuffer buffer = new StringBuffer("Hello World");
			Assert.IsTrue(buffer.Seek(6));
			Assert.AreEqual('W', (char)buffer.Read());
			Assert.AreEqual('o', (char)buffer.Read());
			Assert.AreEqual('r', (char)buffer.Read());
			Assert.AreEqual('l', (char)buffer.Read());
			Assert.AreEqual('d', (char)buffer.Read());
			Assert.IsTrue(buffer.Seek(0));
			Assert.AreEqual('H', (char)buffer.Read());
			Assert.AreEqual('e', (char)buffer.Read());
			Assert.AreEqual('l', (char)buffer.Read());
			Assert.AreEqual('l', (char)buffer.Read());
			Assert.AreEqual('o', (char)buffer.Read());
			Assert.IsFalse(buffer.Seek(-1));
			Assert.IsFalse(buffer.Seek(12));
			Assert.IsFalse(buffer.Seek(11));
		}
	}
}
