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

using BlackBox.Formatting;
using NUnit.Framework;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public class FormatTokenTests
	{
		[Test]
		public void FormatToken_CreateLiteralToken()
		{
			FormatToken token = FormatToken.CreateLiteral("Hello World!");
			Assert.AreEqual(token.Value, "Hello World!");
		}

		[Test]
		public void FormatToken_CreateSymbolToken()
		{
			FormatToken token = FormatToken.CreateWhitespace('?');
			Assert.AreEqual(token.Value, "?");
		}

		[Test]
		public void FormatToken_CreateWhitespaceToken()
		{
			FormatToken token = FormatToken.CreateWhitespace('\t');
			Assert.AreEqual(token.Value, "\t");
		}

		[Test]
		public void TokenExtensions_IsWhitespace()
		{
			FormatToken token = FormatToken.CreateWhitespace('\t');
			Assert.AreEqual(token.Value, "\t");
			Assert.IsTrue(token.IsWhitespace());
			Assert.IsFalse(token.IsSymbol());
			Assert.IsFalse(token.IsSymbol('@'));
			Assert.IsFalse(token.IsSymbol('¤'));
			Assert.IsFalse(token.IsLiteral());
		}

		[Test]
		public void TokenExtensions_IsLiteral()
		{
			FormatToken token = FormatToken.CreateLiteral("Hello World!");
			Assert.AreEqual(token.Value, "Hello World!");
			Assert.IsTrue(token.IsLiteral());
			Assert.IsFalse(token.IsWhitespace());
			Assert.IsFalse(token.IsSymbol());
			Assert.IsFalse(token.IsSymbol('@'));
			Assert.IsFalse(token.IsSymbol('¤'));
		}

		[Test]
		public void TokenExtensions_IsSymbol()
		{
			FormatToken token = FormatToken.CreateSymbol('@');
			Assert.AreEqual(token.Value, "@");
			Assert.IsTrue(token.IsSymbol());
			Assert.IsTrue(token.IsSymbol('@'));
			Assert.IsFalse(token.IsSymbol('¤'));
			Assert.IsFalse(token.IsLiteral());
			Assert.IsFalse(token.IsWhitespace());
		}
	}
}
