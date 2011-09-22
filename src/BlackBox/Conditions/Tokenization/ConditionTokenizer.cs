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
using System.Globalization;
using System.Text;

namespace BlackBox.Conditions
{
	internal sealed class ConditionTokenizer
	{
		private readonly StringBuffer _buffer;

		internal ConditionTokenizer(StringBuffer buffer)
		{
			_buffer = buffer;
		}

		internal static ConditionTokenBuffer Tokenize(string source)
		{
			StringBuffer buffer = new StringBuffer(source);
			ConditionTokenizer tokenizer = new ConditionTokenizer(buffer);
			return new ConditionTokenBuffer(tokenizer.Tokenize());
		}

		internal ConditionToken[] Tokenize()
		{
			_buffer.Seek(0);
			List<ConditionToken> tokens = new List<ConditionToken>();
			while (_buffer.Peek() != -1)
			{
				ConditionToken token = ParseToken();
				tokens.Add(token);
			}
			return tokens.ToArray();
		}

		private ConditionToken ParseToken()
		{
			// Consume whitespace.
			this.ConsumeWhitespace();

			char character = (char)_buffer.Peek();
			if ((int)character == StringBuffer.EndOfBuffer)
			{
				return null;
			}

			if (char.IsNumber(character))
			{
				return this.ParseNumber();
			}
			else if (character == '\'')
			{
				return this.ParseString();
			}
			else if (char.IsLetter(character))
			{
				return this.ParseKeyword();
			}

			// This should be a symbol.
			return this.ParseSymbol();
		}

		private void ConsumeWhitespace()
		{
			int ch;
			while ((ch = _buffer.Peek()) != -1)
			{
				if (!Char.IsWhiteSpace((char)ch))
				{
					break;
				}
				_buffer.Read();
			}
		}

		private ConditionToken ParseString()
		{
			StringBuilder accumulator = new StringBuilder();
			_buffer.Read(); // Consume the '
			char character = (char)_buffer.Peek();
			while (character != '\'')
			{
				accumulator.Append(character);
				_buffer.Read();
				if (_buffer.Peek() == -1)
				{
					break;
				}
				character = (char)_buffer.Peek();
			}

			if (character != '\'')
			{
				throw new ConditionException("Unterminated string literal.");
			}

			_buffer.Read();
			return new ConditionToken(ConditionTokenType.String, accumulator.ToString());
		}

		private ConditionToken ParseKeyword()
		{
			StringBuilder accumulator = new StringBuilder();
			char character = (char)_buffer.Peek();
			while (char.IsLetter(character) || character == '-')
			{
				accumulator.Append(character);
				_buffer.Read();
				if (_buffer.Peek() == -1)
				{
					break;
				}
				character = (char)_buffer.Peek();
			}

			string value = accumulator.ToString();
			if (value.Equals("or", StringComparison.OrdinalIgnoreCase))
			{
				return new ConditionToken(ConditionTokenType.Or, "OR");
			}
			if (value.Equals("and", StringComparison.OrdinalIgnoreCase))
			{
				return new ConditionToken(ConditionTokenType.And, "AND");
			}
			if (value.Equals("not", StringComparison.OrdinalIgnoreCase))
			{
				return new ConditionToken(ConditionTokenType.Not, "NOT");
			}
			return new ConditionToken(ConditionTokenType.Keyword, accumulator.ToString());
		}

		private ConditionToken ParseSymbol()
		{
			char character = (char)_buffer.Read();
			char nextCharacter = (char)_buffer.Peek();

			// Two characters available?
			if ((int)nextCharacter != StringBuffer.EndOfBuffer)
			{
				if (character == '!' && nextCharacter == '=')
				{
					_buffer.Read();
					return new ConditionToken(ConditionTokenType.NotEqualTo, "!=");
				}
				else if (character == '&' && nextCharacter == '&')
				{
					_buffer.Read();
					return new ConditionToken(ConditionTokenType.And, "&&");
				}
				else if (character == '|' && nextCharacter == '|')
				{
					_buffer.Read();
					return new ConditionToken(ConditionTokenType.Or, "||");
				}
				else if (character == '=' && nextCharacter == '=')
				{
					_buffer.Read();
					return new ConditionToken(ConditionTokenType.EqualTo, "==");
				}
				else if (character == '>' && nextCharacter == '=')
				{
					_buffer.Read();
					return new ConditionToken(ConditionTokenType.GreaterThanOrEqualTo, ">=");
				}
				else if (character == '<' && nextCharacter == '=')
				{
					_buffer.Read();
					return new ConditionToken(ConditionTokenType.LessThanOrEqualTo, "<=");
				}
			}

			if (character == '!')
			{
				return new ConditionToken(ConditionTokenType.Not, "!");
			}
			else if (character == '>')
			{
				return new ConditionToken(ConditionTokenType.GreaterThan, ">");
			}
			else if (character == '<')
			{
				return new ConditionToken(ConditionTokenType.LessThan, "<");
			}
			else if (character == '-')
			{
				return new ConditionToken(ConditionTokenType.Minus, "-");
			}
			else if (character == '.')
			{
				return new ConditionToken(ConditionTokenType.Dot, ".");
			}
			else if (character == ',')
			{
				return new ConditionToken(ConditionTokenType.Comma, ",");
			}
			else if (character == '(')
			{
				return new ConditionToken(ConditionTokenType.OpeningParenthesis, "(");
			}
			else if (character == ')')
			{
				return new ConditionToken(ConditionTokenType.ClosingParenthesis, ")");
			}

			// We don't know what this is.
			string message = string.Format(CultureInfo.InvariantCulture, "Unexpected token '{0}' encountered.", character);
			throw new ConditionException(message);
		}

		private ConditionToken ParseNumber()
		{
			int i;
			StringBuilder sb = new StringBuilder();
			char ch = (char)_buffer.Peek();

			sb.Append(ch);
			_buffer.Read();

			while ((i = _buffer.Peek()) != -1)
			{
				ch = (char)i;

				if (char.IsDigit(ch) || (ch == '.'))
				{
					sb.Append((char)_buffer.Read());
				}
				else
				{
					break;
				}
			}

			return new ConditionToken(ConditionTokenType.Number, sb.ToString());
		}
	}
}
