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
using System.Globalization;

namespace BlackBox.Formatting
{
	internal sealed class FormatTokenizer
	{
		private readonly StringBuffer _buffer;

		private FormatTokenizer(StringBuffer buffer)
		{
			_buffer = buffer;
		}

		internal static FormatToken[] Tokenize(string source)
		{
			StringBuffer buffer = new StringBuffer(source);
			FormatTokenizer tokenizer = new FormatTokenizer(buffer);
			return tokenizer.Tokenize();
		}

		private FormatToken[] Tokenize()
		{
			_buffer.Seek(0);
			List<FormatToken> tokens = new List<FormatToken>();
			while (_buffer.Peek() != -1)
			{
				FormatToken token = ParseToken();
				tokens.Add(token);
			}
			return tokens.ToArray();
		}

		private FormatToken ParseToken()
		{
			char character = (char)_buffer.Peek();
			if (character.IsLetter() || character.IsDigit())
			{
				string literal = ReadLiteral();
				return FormatToken.CreateLiteral(literal);
			}
			else if (character.IsWhitespace())
			{
				char whitespace = (char)_buffer.Read();
				return FormatToken.CreateWhitespace(whitespace);
			}
			else
			{
				char symbol = (char)_buffer.Read();                
				if (symbol.Equals('$'))
				{
					// Check if this is a format renderer.
					int peekedValue = _buffer.Peek();
					if (peekedValue != StringBuffer.EndOfBuffer && (char)peekedValue != '$')
					{
						if ((char)peekedValue != '(')
						{
							// Expect a parenthesis here.
							string message = string.Format(CultureInfo.InvariantCulture, "Expected parenthesis but found '{0}'.", (char)peekedValue);
							throw new FormatPatternException(message);
						}

						return FormatToken.CreateFormatRendererBlock();
					}
					else
					{						
						// It was an escaped format renderer ($$).
						// Create a literal of the two tokens.
						StringBuilder accumulator = new StringBuilder();
						accumulator.Append((symbol));
						accumulator.Append((char)_buffer.Read());
						return FormatToken.CreateLiteral(accumulator.ToString());
					}
				}
				return FormatToken.CreateSymbol(symbol);
			}
		}

		private string ReadLiteral()
		{
			StringBuilder accumulator = new StringBuilder();
			char character = (char)_buffer.Peek();
			while (character.IsLetter() || character.IsDigit())
			{
				accumulator.Append(character);
				_buffer.Read();
				if (_buffer.Peek() == -1)
				{
					break;
				}
				character = (char)_buffer.Peek();
			}
			return accumulator.ToString();
		}
	}
}
