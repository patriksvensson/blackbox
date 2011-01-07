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
using System.Reflection;
using System.Collections.ObjectModel;

namespace BlackBox
{
    public sealed class LogFilterCollection : IEnumerable<LogFilter>
    {
        private readonly List<LogFilter> _filters;

        public int Count
        {
            get { return _filters.Count; }
        }

        internal LogFilter this[int index]
        {
            get { return _filters[index]; }
        }

        public LogFilterCollection()
        {
            _filters = new List<LogFilter>();
        }

        #region IEnumerable<LogFilter> Members

        public IEnumerator<LogFilter> GetEnumerator()
        {
            return _filters.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public void Add(LogFilter filter)
        {
            _filters.Add(filter);
        }

        public void AddRange(IEnumerable<LogFilter> filters)
        {
            if (filters != null)
            {
                foreach (LogFilter filter in filters)
                {
                    _filters.Add(filter);
                }
            }
        }

        public bool Remove(LogFilter filter)
        {
            return _filters.Remove(filter);
        }

        public void Clear()
        {
            _filters.Clear();
        }

        internal LogFilterResult Evaluate(ILogEntry entry)
        {
            foreach (LogFilter filter in _filters)
            {
				LogFilterResult result = filter.Evaluate(entry);
				if (result != LogFilterResult.Neutral)
                {
					return result;
                }
            }
            return LogFilterResult.Neutral;            
        }
    }
}
