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
    public class LogSinkNode : HierarchyNode
    {
        private readonly LogSink _sink;
        private readonly HierarchyNode _owner;

        public LogSink Sink
        {
            get { return _sink; }
        }

        public HierarchyNode Owner
        {
            get { return _owner; }
        }

        public override bool CanRemove
        {
            get { return this.Sink.Filters.Count == 0; }
        }

        public override string Text
        {
            get
            {
                // No name?
                if (string.IsNullOrEmpty(_sink.Name))
                {
                    // Return the type of the sink.
                    return _sink.GetType().Name;
                }
                else
                {
                    // Return a pretty name.
                    return string.Format("{0} ({1})", _sink.GetType().Name, _sink.Name);
                }
            }
        }

        public LogSinkNode(HierarchyNodeProvider provider, HierarchyNode owner, LogSink sink)
            : base(provider)
        {
            _owner = owner;
            _sink = sink;            
        }

		public override object GetDataObject()
		{
			return _sink;
		}

        public override bool Remove()
        {
            if (this.CanRemove)
            {
                if (this.Owner is LogSinkCollectionNode)
                {
                    // Remove the sink from the configuration.
                    ((LogSinkCollectionNode)this.Owner).Configuration.Sinks.Remove(this.Sink);
                    this.RefreshParent();
                    return true;
                }
                else if (this.Owner is LogSinkProxyNode)
                {
                    // Remove the sink from the configuration.
                    ((LogSinkProxyNode)this.Owner).Proxy.Sinks.Remove(this.Sink);
                    this.RefreshParent();
                    return true;
                }
            }
            return false;
        }
    }
}
