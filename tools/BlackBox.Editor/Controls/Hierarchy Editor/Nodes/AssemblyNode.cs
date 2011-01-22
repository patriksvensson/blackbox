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

namespace BlackBox.Editor
{
    public class AssemblyNode : HierarchyNode
    {
        private readonly Assembly _assembly;
        private readonly HierarchyNode _owner;

        public Assembly Assembly
        {
            get { return _assembly; }
        }

        public HierarchyNode Owner
        {
            get { return _owner; }
        }

        public override bool CanRemove
        {
            get 
            {
                // TODO: Check so there's no components this assembly own in the configuration.
                return true; 
            }
        }

        public override string Text
        {
            get
            {
                string name = _assembly.GetName().Name;
                string version = _assembly.GetName().Version.ToString(4);
                return string.Concat(name, " (", version, ")");
            }
        }

        public AssemblyNode(HierarchyNodeProvider provider, HierarchyNode owner, Assembly filter)
            : base(provider)
        {
            _owner = owner;
            _assembly = filter;
        }

        public override object GetDataObject()
        {
            return _assembly;
        }

        public override bool Remove()
        {
            if (this.CanRemove)
            {
                if (this.Owner is AssemblyCollectionNode)
                {
                    ((AssemblyCollectionNode)this.Owner).Configuration.Assemblies.Remove(this.Assembly);
                    this.RefreshParent();
                    return true;
                }                
            }
            return false;
        }
    }
}
