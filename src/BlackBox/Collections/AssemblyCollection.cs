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
    public sealed class AssemblyCollection : IEnumerable<Assembly>
    {
        private readonly List<Assembly> _assemblies;

        public int Count
        {
            get { return _assemblies.Count; }
        }

        internal Assembly this[int index]
        {
            get { return _assemblies[index]; }
        }

        public AssemblyCollection()
        {
            _assemblies = new List<Assembly>();
        }

        #region IEnumerable<LogSink> Members

        public IEnumerator<Assembly> GetEnumerator()
        {
            return _assemblies.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        public void Add(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

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

        public bool Remove(Assembly assembly)
        {
            return _assemblies.Remove(assembly);
        }

        public void Clear()
        {
            _assemblies.Clear();
        }
    }
}
