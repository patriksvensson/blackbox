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

namespace BlackBox
{
    internal sealed class StringBuffer
    {
        #region Private Fields

        private readonly string _content;
        private readonly int _length;
        private int _position;

        #endregion

        #region Constants

        internal const int EndOfBuffer = -1;

        #endregion

        #region Properties

        internal string Content
        {
            get { return _content; }
        }

        internal int Length
        {
            get { return _length; }
        }

        internal int Position
        {
            get { return _position; }
        }

        #endregion

        #region Construction

        internal StringBuffer(string content)
        {
            _content = content;
            _length = content.Length;
            _position = 0;
        }

        #endregion

        internal int Peek()
        {
            if (_position < 0 || _position >= _length)
            {
                return -1;
            }
            return _content[_position];
        }

        internal int Read()
        {
            if (_position < 0 || _position >= _length)
            {
                return -1;
            }
            int value = _content[_position];
            _position++;
            return value;
        }

        internal void Rewind()
        {
            _position = 0;
        }

        internal bool Seek(int position)
        {
            if (position < 0 || position >= _length)
            {
                return false;
            }
            _position = position;
            return true;
        }
    }
}
