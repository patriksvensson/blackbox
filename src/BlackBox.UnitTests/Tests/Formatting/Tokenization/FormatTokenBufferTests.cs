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
using BlackBox.Formatting;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public class FormatTokenBufferTests
	{
		[Test]
		public void FormatTokenBuffer_Create()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("Hello World"),
				FormatToken.CreateSymbol('@')
			});

			Assert.AreEqual(2, buffer.Length);
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void FormatTokenBuffer_Peek()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("Hello World")
			});

			Assert.AreEqual(0, buffer.Position);
			Assert.AreEqual("Hello World", buffer.Peek().Value);
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void FormatTokenBuffer_PeekAhead()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("Hello World"),
				FormatToken.CreateSymbol('@')
			});

			Assert.AreEqual(0, buffer.Position);
			Assert.AreEqual("Hello World", buffer.Peek().Value);
			Assert.AreEqual("@", buffer.PeekAhead(1).Value);
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void FormatTokenBuffer_Consume()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("0"),
				FormatToken.CreateLiteral("1"),
				FormatToken.CreateLiteral("2"),
				FormatToken.CreateLiteral("3")
			});

			Assert.AreEqual(0, buffer.Position);
			buffer.Consume(2);
			Assert.AreEqual(2, buffer.Position);
			Assert.AreEqual("2", buffer.Read().Value);
			Assert.AreEqual(3, buffer.Position);
		}

		[Test]
		public void FormatTokenBuffer_Read()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("Hello World")
			});


			Assert.AreEqual(0, buffer.Position);
			Assert.AreEqual("Hello World", buffer.Read().Value);
			Assert.AreEqual(1, buffer.Position);
		}

		[Test]
		public void TokenBuffer_Seek()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("Hello World"),
				FormatToken.CreateSymbol('@')
			});

			Assert.AreEqual(0, buffer.Position);
			Assert.AreEqual("Hello World", buffer.Read().Value);
			Assert.AreEqual(1, buffer.Position);
			Assert.IsTrue(buffer.Seek(0));
			Assert.AreEqual(0, buffer.Position);
			Assert.IsFalse(buffer.Seek(2));
			Assert.AreEqual(0, buffer.Position);
		}

		[Test]
		public void TokenBuffer_Rewind()
		{
			Buffer<FormatToken> buffer = new FormatTokenBuffer(new List<FormatToken> 
			{
				FormatToken.CreateLiteral("Hello World"),
				FormatToken.CreateSymbol('@'),
				FormatToken.CreateWhitespace('\t')
			});

			buffer.Seek(2);
			Assert.AreEqual(2, buffer.Position);
			buffer.Rewind();
			Assert.AreEqual(0, buffer.Position);
		}
	}
}
