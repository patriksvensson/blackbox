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
    /// <summary>
    /// Represents a strongly typed list of assemblies that can be accessed by index.
    /// </summary>
    public sealed class AssemblyCollection : IEnumerable<Assembly>
    {
        private readonly List<Assembly> _assemblies;

        /// <summary>
        /// Gets the number of assemblies contained in the collection.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _assemblies.Count; }
        }

        /// <summary>
        /// Gets the <see cref="System.Reflection.Assembly"/> at the specified index.
        /// </summary>
        /// <value></value>
        internal Assembly this[int index]
        {
            get { return _assemblies[index]; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCollection"/> class.
        /// </summary>
        public AssemblyCollection()
        {
            _assemblies = new List<Assembly>();
        }

        #region IEnumerable<LogSink> Members

        /// <summary>
        /// Gets the enumerator that iterates through the collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Assembly> GetEnumerator()
        {
            return _assemblies.GetEnumerator();
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

        /// <summary>
        /// Adds the specified assembly to the collection.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public void Add(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

        /// <summary>
        /// Adds the specified assemblies to the collection.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void AddRange(IEnumerable<Assembly> assemblies)
        {
            if (assemblies != null)
            {
                foreach (Assembly assembly in assemblies)
                {
                    _assemblies.Add(assembly);
                }
            }
        }

        /// <summary>
        /// Removes the specified assembly from the collection.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public bool Remove(Assembly assembly)
        {
            return _assemblies.Remove(assembly);
        }

        /// <summary>
        /// Removes all assemblies from the collection.
        /// </summary>
        public void Clear()
        {
            _assemblies.Clear();
        }
    }
}
