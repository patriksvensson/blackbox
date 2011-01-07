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
using BlackBox.Formatting;

namespace BlackBox.UnitTests.Tests.Formatting
{
	[TestFixture]
	public sealed class FormatTokenizerTests
	{
		[Test]
		public void FormatTokenizer_ParseLiteral()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("HelloWorld");
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(FormatTokenType.Literal, result[0].TokenType);
			Assert.AreEqual("HelloWorld", result[0].Value);
		}

		[Test]
		public void FormatTokenizer_ParseNumberAndLetters()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("123HelloWorld456");
			Assert.IsNotNull(result);
			Assert.AreEqual(1, result.Length);
			Assert.AreEqual(FormatTokenType.Literal, result[0].TokenType);
			Assert.AreEqual("123HelloWorld456", result[0].Value);
		}

		[Test]
		public void FormatTokenizer_ParseEscapedBackslash()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("Hello World \\$(time)");
			Assert.IsNotNull(result);
			Assert.AreEqual(9, result.Length);
			Assert.AreEqual(FormatTokenType.Literal, result[0].TokenType); // Hello
			Assert.AreEqual(FormatTokenType.Whitespace, result[1].TokenType);
			Assert.AreEqual(FormatTokenType.Literal, result[2].TokenType); // World
			Assert.AreEqual(FormatTokenType.Whitespace, result[3].TokenType);
			Assert.AreEqual(FormatTokenType.Symbol, result[4].TokenType); // \
			Assert.AreEqual(FormatTokenType.FormatRenderer, result[5].TokenType); // $
			Assert.AreEqual(FormatTokenType.Symbol, result[6].TokenType); // (
			Assert.AreEqual(FormatTokenType.Literal, result[7].TokenType); // time
			Assert.AreEqual(FormatTokenType.Symbol, result[8].TokenType); // )
		}

		[Test]
		public void FormatTokenizer_ParseEscapedRenderer()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("Hello World $$(time)");
			Assert.IsNotNull(result);
			Assert.AreEqual(8, result.Length);
			Assert.AreEqual(FormatTokenType.Literal, result[0].TokenType); // Hello
			Assert.AreEqual(FormatTokenType.Whitespace, result[1].TokenType);
			Assert.AreEqual(FormatTokenType.Literal, result[2].TokenType); // World
			Assert.AreEqual(FormatTokenType.Whitespace, result[3].TokenType);
			Assert.AreEqual(FormatTokenType.Literal, result[4].TokenType); // $$
			Assert.AreEqual(FormatTokenType.Symbol, result[5].TokenType); // (
			Assert.AreEqual(FormatTokenType.Literal, result[6].TokenType); // time
			Assert.AreEqual(FormatTokenType.Symbol, result[7].TokenType); // )
		}

		[Test]
		public void FormatTokenizer_ParseWhitespace()
		{
			FormatToken[] result = FormatTokenizer.Tokenize(" \t");
			Assert.IsNotNull(result);
			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(FormatTokenType.Whitespace, result[0].TokenType);
			Assert.AreEqual(" ", result[0].Value);
			Assert.AreEqual(FormatTokenType.Whitespace, result[1].TokenType);
			Assert.AreEqual("\t", result[1].Value);
		}

		[Test]
		public void FormatTokenizer_ParseSymbols()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("*()_");
			Assert.IsNotNull(result);
			Assert.AreEqual(4, result.Length);
			Assert.AreEqual(FormatTokenType.Symbol, result[0].TokenType);
			Assert.AreEqual("*", result[0].Value);
			Assert.AreEqual(FormatTokenType.Symbol, result[1].TokenType);
			Assert.AreEqual("(", result[1].Value);
			Assert.AreEqual(FormatTokenType.Symbol, result[2].TokenType);
			Assert.AreEqual(")", result[2].Value);
			Assert.AreEqual(FormatTokenType.Symbol, result[3].TokenType);
			Assert.AreEqual("_", result[3].Value);
		}

		[Test]
		[ExpectedException(ExpectedException = typeof(FormatPatternException), ExpectedMessage = "Expected parenthesis but found 't'.")]
		public void FormatTokenizer_ParseFormatParserMissingStartParenthesis()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("$time)");
		}

		[Test]
		public void FormatTokenizer_ParseFormatString()
		{
			FormatToken[] result = FormatTokenizer.Tokenize("[$(@rot13(format='HH:mm:ss'))] [$(@message)]");
			Assert.IsNotNull(result);
			Assert.AreEqual(26, result.Length);
			Assert.AreEqual(FormatTokenType.Symbol, result[0].TokenType); // [
			Assert.AreEqual(FormatTokenType.FormatRenderer, result[1].TokenType); // $
			Assert.AreEqual(FormatTokenType.Symbol, result[2].TokenType); // (
			Assert.AreEqual(FormatTokenType.Symbol, result[3].TokenType); // @
			Assert.AreEqual(FormatTokenType.Literal, result[4].TokenType); // rot13
			Assert.AreEqual(FormatTokenType.Symbol, result[5].TokenType); // (
			Assert.AreEqual(FormatTokenType.Literal, result[6].TokenType); // format
			Assert.AreEqual(FormatTokenType.Symbol, result[7].TokenType); // =
			Assert.AreEqual(FormatTokenType.Symbol, result[8].TokenType); // '
			Assert.AreEqual(FormatTokenType.Literal, result[9].TokenType); // HH
			Assert.AreEqual(FormatTokenType.Symbol, result[10].TokenType); // :
			Assert.AreEqual(FormatTokenType.Literal, result[11].TokenType); // mm
			Assert.AreEqual(FormatTokenType.Symbol, result[12].TokenType); // :
			Assert.AreEqual(FormatTokenType.Literal, result[13].TokenType); // ss
			Assert.AreEqual(FormatTokenType.Symbol, result[14].TokenType); // '
			Assert.AreEqual(FormatTokenType.Symbol, result[15].TokenType); // )
			Assert.AreEqual(FormatTokenType.Symbol, result[16].TokenType); // )
			Assert.AreEqual(FormatTokenType.Symbol, result[17].TokenType); // ]
			Assert.AreEqual(FormatTokenType.Whitespace, result[18].TokenType); 
			Assert.AreEqual(FormatTokenType.Symbol, result[19].TokenType); // [
			Assert.AreEqual(FormatTokenType.FormatRenderer, result[20].TokenType); // $
			Assert.AreEqual(FormatTokenType.Symbol, result[21].TokenType); // (
			Assert.AreEqual(FormatTokenType.Symbol, result[22].TokenType); // @
			Assert.AreEqual(FormatTokenType.Literal, result[23].TokenType); // message
			Assert.AreEqual(FormatTokenType.Symbol, result[24].TokenType); // )
			Assert.AreEqual(FormatTokenType.Symbol, result[25].TokenType); // ]
		}
	}
}