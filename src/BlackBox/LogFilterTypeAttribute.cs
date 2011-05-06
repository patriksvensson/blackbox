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

namespace BlackBox
{
	/// <summary>
	/// Attribute that is used to decorate log filters.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class LogFilterTypeAttribute : Attribute
	{
		private readonly string _name;

		/// <summary>
		/// Gets the name of the log filter.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LogFilterTypeAttribute"/> class.
		/// </summary>
		/// <param name="name">The name of the log filter.</param>
		public LogFilterTypeAttribute(string name)
		{
			_name = name;
		}
	}
}
