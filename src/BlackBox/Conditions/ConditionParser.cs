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
using System.Globalization;

namespace BlackBox.Conditions
{
	internal sealed class ConditionParser
	{
		private readonly ConditionTokenBuffer _tokens;

		internal ConditionParser(ConditionTokenBuffer buffer)
		{
			_tokens = buffer;
		}

		internal static ConditionExpression ParseCondition(string condition)
		{
			ConditionTokenBuffer buffer = ConditionTokenizer.Tokenize(condition);
			ConditionParser parser = new ConditionParser(buffer);
			return parser.ParseBooleanExpression();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private ConditionExpression ParseExpression()
		{
			ConditionExpression expression = this.ParseBooleanExpression();
			if (_tokens.Current != null)
			{
				throw new ConditionException("Could not parse expression.");
			}
			return expression;
		}

		private ConditionExpression ParseBooleanExpression()
		{
			return this.ParseBooleanOrExpression();
		}

		private ConditionExpression ParseBooleanOrExpression()
		{
			ConditionExpression expression = this.ParseBooleanAndExpression();
			while (_tokens.Current != null && _tokens.Current.TokenType == ConditionTokenType.Or)
			{
				_tokens.Read();
				expression = new OrExpression(expression, this.ParseBooleanAndExpression());
			}
			return expression;
		}

		private ConditionExpression ParseBooleanAndExpression()
		{
			if (_tokens.Current == null)
			{
				throw new ConditionException("Expected token but encountered null.");
			}

			ConditionExpression expression = this.ParseBooleanPredicate();
			while (_tokens.Current != null && _tokens.Current.TokenType == ConditionTokenType.And)
			{
				_tokens.Read();
				expression = new AndExpression(expression, this.ParseBooleanPredicate());
			}
			return expression;
		}

		private ConditionExpression ParseBooleanPredicate()
		{
			if (_tokens.Current == null)
			{
				throw new ConditionException("Expected token but encountered end of expression.");
			}

			if (_tokens.Current.TokenType == ConditionTokenType.Not)
			{
				_tokens.Read();
				return new NotExpression(this.ParseBooleanPredicate());
			}
			return this.ParseBooleanRelation();
		}

		private ConditionExpression ParseBooleanRelation()
		{
			// Parse the left side literal in the relation.
			ConditionExpression expression = this.ParseLiteralExpression();

			// Still got tokens?
			if (_tokens.Current != null)
			{
				// Equality?
				if (_tokens.Current.TokenType == ConditionTokenType.EqualTo)
				{
					_tokens.Read();
					return new RelationalExpression(expression, this.ParseLiteralExpression(), RelationalOperator.EqualTo);
				}
				// Greater than?
				if (_tokens.Current.TokenType == ConditionTokenType.GreaterThan)
				{
					_tokens.Read();
					return new RelationalExpression(expression, this.ParseLiteralExpression(), RelationalOperator.GreaterThan);
				}
				// Greater than or equal to?
				if (_tokens.Current.TokenType == ConditionTokenType.GreaterThanOrEqualTo)
				{
					_tokens.Read();
					return new RelationalExpression(expression, this.ParseLiteralExpression(), RelationalOperator.GreaterThanOrEqualTo);
				}
				// Less than?
				if (_tokens.Current.TokenType == ConditionTokenType.LessThan)
				{
					_tokens.Read();
					return new RelationalExpression(expression, this.ParseLiteralExpression(), RelationalOperator.LessThan);
				}
				// Less than or equal to?
				if (_tokens.Current.TokenType == ConditionTokenType.LessThanOrEqualTo)
				{
					_tokens.Read();
					return new RelationalExpression(expression, this.ParseLiteralExpression(), RelationalOperator.LessThanOrEqualTo);
				}
			}

			return expression;
		}

		private ConditionExpression ParseLiteralExpression()
		{
			if (_tokens.Current == null)
			{
				throw new ConditionException("Expected token but encountered end of expression.");
			}

			// Parenthesis?
			if (_tokens.Current.TokenType == ConditionTokenType.OpeningParenthesis)
			{
				_tokens.Read();
				ConditionExpression expression = this.ParseBooleanExpression();
				_tokens.ExpectToken(ConditionTokenType.ClosingParenthesis);
				return expression;
			}

			// Number?
			if (_tokens.Current.TokenType == ConditionTokenType.Number)
			{
				string value = _tokens.Current.Value;
				_tokens.Read();
				return new ConstantExpression(Int32.Parse(value, CultureInfo.InvariantCulture));
			}

			// String?
			if (_tokens.Current.TokenType == ConditionTokenType.String)
			{
				string value = _tokens.Current.Value;
				_tokens.Read();
				return new ConstantExpression(value);
			}

			// Keyword?
			if (_tokens.Current.TokenType == ConditionTokenType.Keyword)
			{
				string value = _tokens.Current.Value;
				_tokens.Read();

				if (value.Equals("true", StringComparison.OrdinalIgnoreCase))
				{
					return new ConstantExpression(true);
				}
				else if (value.Equals("false", StringComparison.OrdinalIgnoreCase))
				{
					return new ConstantExpression(false);
				}
				else if (value.Equals("null", StringComparison.OrdinalIgnoreCase))
				{
					return new ConstantExpression(null);
				}
				else if (value.Equals("message", StringComparison.OrdinalIgnoreCase))
				{
					return new LogMessageExpression();
				}
				else if (value.Equals("level", StringComparison.OrdinalIgnoreCase))
				{
					return new LogLevelExpression(true /* Log level number */);
				}
				else if (value.Equals("levelname", StringComparison.OrdinalIgnoreCase))
				{
					return new LogLevelExpression(false /* Log level name */);
				}
				else if (value.Equals("has-exception", StringComparison.OrdinalIgnoreCase))
				{
					return new HasExceptionExpression();
				}

				// Is the next parameter a parenthesis?
				// This is one of our special functions.
				if (_tokens.Current != null && _tokens.Current.TokenType == ConditionTokenType.OpeningParenthesis)
				{
					// Parse the method!
					throw new NotImplementedException("Methods are not supported (yet).");
				}

				// Unknown keyword found.
				string message = string.Format(CultureInfo.InvariantCulture, "Unknown keyword '{0}' encountered.", value);
				throw new ConditionException(message);
			}

			// We do not know what do do with this token.
			throw new ConditionException("Invalid token encountered when parsing literal expression.");
		}
	}
}
