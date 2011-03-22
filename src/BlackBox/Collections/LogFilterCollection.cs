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
	/// <summary>
	/// Represents a strongly typed list of log filters that can be accessed by index.
	/// </summary>
	public sealed class LogFilterCollection : IEnumerable<LogFilter>
	{
		private readonly List<LogFilter> _filters;

		/// <summary>
		/// Gets the number of log filters in the collection.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get { return _filters.Count; }
		}

		/// <summary>
		/// Gets the <see cref="BlackBox.LogFilter"/> at the specified index.
		/// </summary>
		/// <value></value>
		internal LogFilter this[int index]
		{
			get { return _filters[index]; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogFilterCollection"/> class.
		/// </summary>
		public LogFilterCollection()
		{
			_filters = new List<LogFilter>();
		}

		#region IEnumerable<LogFilter> Members

		/// <summary>
		/// Gets the enumerator that iterates through the collection.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<LogFilter> GetEnumerator()
		{
			return _filters.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion

		#region Initialization

		internal void Initialize(InitializationContext context)
		{
			foreach (LogFilter filter in _filters)
			{
				filter.Initialize(context);
			}
		}

		#endregion

		/// <summary>
		/// Adds the specified filter to the collection.
		/// </summary>
		/// <param name="filter">The filter.</param>
		public void Add(LogFilter filter)
		{
			_filters.Add(filter);
		}

		/// <summary>
		/// Adds the specified log filters to the collection.
		/// </summary>
		/// <param name="filters">The filters.</param>
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

		/// <summary>
		/// Removes the specified log filter from the collection.
		/// </summary>
		/// <param name="filter">The filter.</param>
		/// <returns></returns>
		public bool Remove(LogFilter filter)
		{
			return _filters.Remove(filter);
		}

		/// <summary>
		/// Removes all log filters from the collection.
		/// </summary>
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
