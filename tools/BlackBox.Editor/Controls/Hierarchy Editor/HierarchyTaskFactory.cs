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
using BlackBox.Editor.Properties;

namespace BlackBox.Editor
{
	public static class HierarchyTaskFactory
	{
		public static HierarchyTask AddSinkTask(HierarchyNode owner)
		{
            return new HierarchyTask("Add Sink...", owner, HierarchyTaskMethods.AddLogSink).SetGroup(0);
		}

        public static HierarchyTask AddFilterTask(HierarchyNode owner)
        {
            return new HierarchyTask("Add Filter...", owner, HierarchyTaskMethods.AddFilter)
                .SetGroup(0);
        }

        public static HierarchyTask AddAssemblyTask(HierarchyNode owner)
        {
            return new HierarchyTask("Add Assembly...", owner, x => { })
                .SetGroup(0).IsEnabled(false);
        }

        public static HierarchyTask RemoveSinkTask(HierarchyNode owner)
        {
            return new HierarchyTask("Remove Sink...", owner, HierarchyTaskMethods.Remove)
                .SetGroup(1).SetImage(Resources.Delete).IsEnabled(owner.CanRemove);
        }

        public static HierarchyTask RemoveFilterTask(HierarchyNode owner)
        {
            return new HierarchyTask("Remove Filter...", owner, HierarchyTaskMethods.Remove)
                .SetGroup(1).SetImage(Resources.Delete).IsEnabled(owner.CanRemove);
        }

        public static HierarchyTask RemoveAssemblyTask(HierarchyNode owner)
        {
            return new HierarchyTask("Remove Assembly...", owner, HierarchyTaskMethods.Remove)
                .SetGroup(1).SetImage(Resources.Delete).IsEnabled(owner.CanRemove);
        }
	}
}
