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
    public class LogFilterNode : HierarchyNode
    {
        private readonly LogFilter _filter;
        private readonly HierarchyNode _owner;

        public LogFilter Filter
        {
            get { return _filter; }
        }

        public HierarchyNode Owner
        {
            get { return _owner; }
        } 

        public override string Text
        {
            get { return _filter.GetType().Name; }
        }

        public override bool CanRemove
        {
            get { return true; }
        }

        public LogFilterNode(HierarchyNodeProvider provider, HierarchyNode owner, LogFilter filter)
            : base(provider)
        {
            _owner = owner;
            _filter = filter;
        }

		public override object GetDataObject()
		{
			return _filter;
		}

        public override bool Remove()
        {
            if (this.CanRemove)
            {
                if (this.Owner is LogSinkNode)
                {
                    // Remove the filter from the sink.
                    ((LogSinkNode)this.Owner).Sink.Filters.Remove(this.Filter);
                    this.RefreshParent();
                    return true;
                }
                else if (this.Owner is LogSinkProxyNode)
                {
                    // Remove the filter from the proxy.
                    ((LogSinkProxyNode)this.Owner).Proxy.Filters.Remove(this.Filter);
                    this.RefreshParent();
                    return true;
                }
                else if (this.Owner is LogFilterCollectionNode)
                {
                    // Remove the filter from the configuration.
                    ((LogFilterCollectionNode)this.Owner).Configuration.Filters.Remove(this.Filter);
                    this.RefreshParent();
                    return true;
                }                
            }
            return false;
        }  
    }
}
