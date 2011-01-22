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
using System.Drawing;

namespace BlackBox.Editor
{
    public static class HierarchyTaskExtensions
    {
        /// <summary>
        /// Sets the group.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public static HierarchyTask SetGroup(this HierarchyTask task, int group)
        {
            task.Group = group;
            return task;
        }

        /// <summary>
        /// Sets the sort order.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public static HierarchyTask SetSortOrder(this HierarchyTask task, int sortOrder)
        {
            task.SortOrder = sortOrder;
            return task;
        }

        /// <summary>
        /// Sets the image.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static HierarchyTask SetImage(this HierarchyTask task, Image image)
        {
            task.Image = image;
            return task;
        }

        /// <summary>
        /// Determines whether the specified task is enabled.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        /// <returns></returns>
        public static HierarchyTask IsEnabled(this HierarchyTask task, bool enabled)
        {
            task.Enabled = enabled;
            return task;
        }

        /// <summary>
        /// Determines whether the specified task is visible.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        /// <returns></returns>
        public static HierarchyTask IsVisible(this HierarchyTask task, bool visible)
        {
            task.Visible = visible;
            return task;
        }
    }
}
