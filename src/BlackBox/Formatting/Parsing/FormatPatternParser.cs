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

using System.Collections.Generic;
using System.Text;

namespace BlackBox.Formatting
{
	internal sealed class FormatPatternParser
	{
		private readonly FormatTokenBuffer _buffer;

		private FormatPatternParser(FormatTokenBuffer buffer)
		{
			_buffer = buffer;
		}

		internal static FormatPatternNode[] Parse(string source)
		{
			FormatToken[] tokens = FormatTokenizer.Tokenize(source);
			FormatTokenBuffer buffer = new FormatTokenBuffer(tokens);
			FormatPatternParser parser = new FormatPatternParser(buffer);
			return parser.Parse();
		}

		private FormatPatternNode[] Parse()
		{
			_buffer.Rewind();
			var result = new List<FormatPatternNode>();
			while (_buffer.Peek() != null)
			{
				FormatToken current = _buffer.Peek();
				if (current.IsFormatRendererBlock())
				{
					if (_buffer.PeekAhead(1).IsSymbol('('))
					{
						// Valid parenthesis block here?
						bool isValidBlock = this.IsValidParenthesisBlock(_buffer.Position + 1);
						if (isValidBlock)
						{
							_buffer.Consume(2); // Consume the renderer start.
							FormatPatternNode renderer = this.ParseRenderer();
							result.Add(renderer);
							if (!_buffer.Peek().IsSymbol(')'))
							{
								throw new FormatPatternException("Encountered non terminated block.");
							}
							_buffer.Read(); // Consume the end parenthesis.
							continue;
						}
						else
						{
							throw new FormatPatternException("Format renderer block is not well formed.");
						}
					}
				}

				// Parse it as a literal.
				FormatPatternNode literal = this.ParseLiteral();
				result.Add(literal);
			}
			return result.ToArray();
		}

		private FormatRendererNode ParseRenderer()
		{
			// A renderer can have six formats:
			//
			// 1) renderer()
			// 2) renderer(arg1=value,arg2=value)
			// 3) renderer(otherrenderer(...))
			// 4) renderer(otherrenderer(...),arg1=value,arg2=value)
			// 5) renderer(arg1=value,otherrenderer(...),arg2=value);
			// 6) renderer(arg1=value,arg2=value,otherrenderer(...))
			//
			// We need to parse the "renderer" part (which should be a word)
			// and then check the content of the parenthesis block to see if
			// there is another renderer there or just arguments.
			// If there is another renderer there we recurse into "ParseRenderer",
			// and wrap the parsed renderer in a transformer. Otherwise we just add the
			// parsed arguments to the renderer and return it.

			// First argument is always a literal.
			FormatToken rendererName = _buffer.Read();
			if (rendererName.IsLiteral())
			{
				// Now we must encounter a start parenthesis.
				FormatToken parenthesis = _buffer.Read();
				if (parenthesis.IsSymbol('('))
				{
					FormatRendererNode childRenderer = null;
					List<FormatArgumentNode> arguments = new List<FormatArgumentNode>();

					while (!_buffer.Peek().IsSymbol(')'))
					{
						// End of buffer?
						if (_buffer.Peek() == null)
						{
							throw new FormatPatternException("Unexpected end of buffer.");
						}

						// Is there a literal (child renderer or argument)?
						if (_buffer.Peek().IsLiteral())
						{
							// Child renderer?
							if (_buffer.PeekAhead(1).IsSymbol('('))
							{
								#region Parse Child Renderer
								if (childRenderer != null)
								{
									throw new FormatPatternException("The transformer '{0}' contains more than one nested renderer.", rendererName.Value);
								}
								childRenderer = this.ParseRenderer();
								#endregion
							}
							// Arguments?
							else if (_buffer.PeekAhead(1).IsSymbol('='))
							{
								#region Parse Argument
								FormatArgumentNode argument = this.ParseArgument();
								if (argument == null)
								{
									return null;
								}
								arguments.Add(argument);
								#endregion
							}
							else
							{
								throw new FormatPatternException("Neither renderer nor argument. Don't know what to do with this.");
							}
						}
						else
						{
							// Now make sure to "eat" comma delimiters and white spaces.
							if (_buffer.Peek().IsSymbol(',') || _buffer.Peek().IsWhitespace())
							{
								_buffer.Read();
							}
							else
							{
								throw new FormatPatternException("Unexpected token ({0}) found when parsing renderer.", _buffer.Peek().Value);
							}
						}
					}

					if (_buffer.Peek().IsSymbol(')'))
					{
						_buffer.Read();
					}
					else
					{
						throw new FormatPatternException("Encountered non terminated parenthesis block.");
					}

					// Build the renderer node.
					return this.BuildRendererFromParts(rendererName.Value, childRenderer, arguments);
				}
			}


			// This means that the renderer is not a valid renderer.
			throw new FormatPatternException("Not a valid renderer.");
		}

		private FormatArgumentNode ParseArgument()
		{
			FormatToken nameToken = FormatToken.CreateLiteral(this.ParseQuotedLiteral().Literal);
			FormatToken equalsToken = _buffer.Read();
			FormatToken valueToken = FormatToken.CreateLiteral(this.ParseQuotedLiteral().Literal);

			#region Sanity Check
			if (!nameToken.IsLiteral())
			{
				throw new FormatPatternException("Could not parse argument name.");
			}
			if (!equalsToken.IsSymbol('='))
			{
				throw new FormatPatternException("Expected equality sign when parsing argument '{0}'.", nameToken.Value);
			}
			if (!valueToken.IsLiteral())
			{
				throw new FormatPatternException("Could not parse argument value for argument '{0}'.", nameToken.Value);
			}
			#endregion

			return new FormatArgumentNode(nameToken.Value, valueToken.Value);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
		private FormatRendererNode BuildRendererFromParts(string rendererName, FormatRendererNode childRenderer, List<FormatArgumentNode> arguments)
		{
			FormatRendererNode renderer = new FormatRendererNode(rendererName);
			if (childRenderer != null)
			{
				renderer = new FormatTransformerNode(rendererName, childRenderer);
			}
			if (arguments != null)
			{
				foreach (FormatArgumentNode argument in arguments)
				{
					renderer.SetArgument(argument);
				}
			}
			return renderer;
		}

		private LiteralNode ParseLiteral()
		{
			StringBuilder accumulator = new StringBuilder();
			accumulator.Append(_buffer.Read().Value);
			while (_buffer.Peek() != null)
			{
				// Did we hit a potential renderer?
				if (_buffer.Peek().IsFormatRendererBlock())
				{
					break;
				}
				accumulator.Append(_buffer.Read().Value);
			}
			return new LiteralNode(accumulator.ToString());
		}

		private LiteralNode ParseQuotedLiteral()
		{
			StringBuilder accumulator = new StringBuilder();
			if (_buffer.Peek().IsSymbol('\''))
			{
				_buffer.Read();
				while (!_buffer.Peek().IsSymbol('\''))
				{
					if (_buffer.Peek() == null)
					{
						throw new FormatPatternException("Encountered unterminated string literal.");
					}
					var token = _buffer.Read();
					accumulator.Append(token.Value);
				}
				_buffer.Read();
				return new LiteralNode(accumulator.ToString());
			}
			else
			{
				var token = _buffer.Read();
				if (token.IsLiteral())
				{
					return new LiteralNode(token.Value);
				}
				else
				{
					throw new FormatPatternException("Expected a literal.");
				}
			}
		}

		private bool IsValidParenthesisBlock(int blockStartPosition)
		{
			// Store the position so we can reset it.
			int originalPosition = _buffer.Position;

			try
			{
				// Seek to where the block starts.
				_buffer.Seek(blockStartPosition);

				// Make sure the block starts correctly.
				if (!_buffer.Peek().IsSymbol('('))
				{
					return false;
				}

				_buffer.Read();
				int blocksOpened = 1;
				while (_buffer.Peek() != null)
				{
					FormatToken token = _buffer.Read();
					if (token.IsSymbol('('))
					{
						blocksOpened++;
					}
					if (token.IsSymbol(')'))
					{
						blocksOpened--;
					}
					if (blocksOpened == 0)
					{
						return true;
					}
				}

				return false;
			}
			finally
			{
				// Rewind the buffer.
				_buffer.Seek(originalPosition);
			}
		}
	}
}
