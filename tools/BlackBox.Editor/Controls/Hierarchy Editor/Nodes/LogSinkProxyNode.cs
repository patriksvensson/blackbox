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

namespace BlackBox.Editor
{
    public class LogSinkProxyNode : HierarchyNode
    {
        private readonly LogSinkProxy _proxy;
        private readonly HierarchyNode _owner;

        public LogSinkProxy Proxy
        {
            get { return _proxy; }
        }

        public HierarchyNode Owner
        {
            get { return _owner; }
        }

        public override bool CanRemove
        {
            get { return _proxy.Sinks.Count == 0; }
        }  

        public override string Text
        {
            get
            {
                // No name?
                if (string.IsNullOrEmpty(_proxy.Name))
                {
                    // Return the type of the sink.
                    return _proxy.GetType().Name;
                }
                else
                {
                    // Return a pretty name.
                    return string.Format("{0} ({1})", _proxy.GetType().Name, _proxy.Name);
                }
            }
        }

        public LogSinkProxyNode(HierarchyNodeProvider provider, HierarchyNode owner, LogSinkProxy proxy)
            : base(provider)
        {
            _owner = owner;
            _proxy = proxy;
        }

		public override object GetDataObject()
		{
			return _proxy;
		}

        public override bool Remove()
        {
            if (this.CanRemove)
            {
                if (this.Owner is LogSinkCollectionNode)
                {
                    // Remove the proxy from the configuration.
                    ((LogSinkCollectionNode)this.Owner).Configuration.Sinks.Remove(this.Proxy);
                    this.RefreshParent();
                    return true;
                }
                else if (this.Owner is LogSinkProxyNode)
                {
                    // Remove the proxy from the configuration.
                    ((LogSinkProxyNode)this.Owner).Proxy.Sinks.Remove(this.Proxy);
                    this.RefreshParent();
                    return true;
                }                
            }
            return false;
        }
    }
}
