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

namespace BlackBox.Formatting
{
	internal sealed class FormatToken
	{
		private readonly FormatTokenType _tokenType;
		private readonly string _value;

		#region Properties

		internal FormatTokenType TokenType
		{
			get { return _tokenType; }
		}

		public string Value
		{
			get { return _value; }
		}

		#endregion

		#region Construction

		private FormatToken(FormatTokenType tokenType, string value)
		{
			_tokenType = tokenType;
			_value = value;
		}

		#endregion

		#region Factory Methods

		internal static FormatToken CreateLiteral(string value)
		{
			return new FormatToken(FormatTokenType.Literal, value);
		}

		internal static FormatToken CreateSymbol(char value)
		{
			return new FormatToken(FormatTokenType.Symbol, value.ToString());
		}

		internal static FormatToken CreateWhitespace(char value)
		{
			return new FormatToken(FormatTokenType.Whitespace, value.ToString());
		}

		internal static FormatToken CreateFormatRendererBlock()
		{
			return new FormatToken(FormatTokenType.FormatRenderer, "$");
		}

		#endregion

		internal bool IsWhitespace()
		{
			return _tokenType == FormatTokenType.Whitespace;
		}

		internal bool IsLiteral()
		{
			return _tokenType == FormatTokenType.Literal;
		}

		internal bool IsFormatRendererBlock()
		{
			return _tokenType == FormatTokenType.FormatRenderer;
		}

		internal bool IsSymbol()
		{
			return _tokenType == FormatTokenType.Symbol;
		}

		internal bool IsSymbol(char symbol)
		{
			return this.IsSymbol() ? _value[0].Equals(symbol) : false;
		}
	}
}
