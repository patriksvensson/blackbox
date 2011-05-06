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

namespace BlackBox
{
	internal abstract class Buffer<T>
		where T : class
	{
		#region Private Fields

		private readonly List<T> _tokens;
		private int _position;

		#endregion

		#region Properties

		internal int Position
		{
			get { return _position; }
		}

		internal int Length
		{
			get { return _tokens.Count; }
		}

		public T Current
		{
			get
			{
				if (_position >= this.Length)
				{
					return null;
				}
				return _tokens[_position];
			}
		}

		#endregion

		#region Construction

		internal Buffer(IEnumerable<T> tokens)
		{
			_tokens = new List<T>(tokens);
			_position = 0;
		}

		#endregion

		internal T Peek()
		{
			if (_position >= this.Length)
			{
				return null;
			}
			return _tokens[_position];
		}

		public T PeekAhead(int index)
		{
			int position = _position + index;
			if (position >= this.Length)
			{
				return null;
			}
			return _tokens[position];
		}

		internal T Read()
		{
			if (_position >= this.Length)
			{
				return null;
			}
			var token = _tokens[_position];
			_position++;
			return token;
		}

		internal bool Consume(int count)
		{
			_position = _position + count;
			return _position < this.Length;
		}

		internal void Rewind()
		{
			_position = 0;
		}

		internal bool Seek(int position)
		{
			if (position < 0 || position >= this.Length)
			{
				return false;
			}
			_position = position;
			return true;
		}
	}
}
