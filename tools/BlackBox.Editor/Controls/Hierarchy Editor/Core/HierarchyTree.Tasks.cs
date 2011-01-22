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
using System.Windows.Forms;

namespace BlackBox.Editor
{
    public partial class HierarchyTree
    {
        /// <summary>
        /// Creates the task context menu strip.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <returns></returns>
        private ContextMenuStrip CreateTaskContextMenuStrip(HierarchyNode info)
        {
            // Create the context menu.
            ContextMenuStrip menu = new ContextMenuStrip();

            // Get the node's tasks.
            var tasks = info.GetTasks().ToList();

            // Iterate through all providers.
            foreach (HierarchyNodeProvider provider in this.Builder.Providers)
            {
                // Get all provider's tasks.
                var providerTasks = provider.GetTasks(this.Builder.Context, info).ToList();
                tasks.AddRange(providerTasks);
            }

            // Add all tasks to the context menu strip.
            this.AddTasksToContextMenuStrip(menu, tasks);

            return menu;
        }

        /// <summary>
        /// Adds the tasks to context menu strip.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="tasks">The tasks.</param>
        private void AddTasksToContextMenuStrip(ContextMenuStrip menu, IEnumerable<HierarchyTask> tasks)
        {
            // Group the tasks by their task group.
            var grouped = from task in tasks
                          group task by task.Group into taskGroup
                          select new { Group = taskGroup.Key, Command = taskGroup };

            int groupIndex = 0;
            foreach (var group in grouped)
            {
                // Iterate through all commands.
                foreach (var task in group.Command.OrderBy(x => x.SortOrder))
                {
                    // Start with a separator? Holy kaw...
                    if (task is HierarchyTaskSeparator && menu.Items.Count == 0)
                    {
                        continue;
                    }

                    // Create the tool strip item.
                    ToolStripItem item = this.CreateContextMenuItem(task);
                    menu.Items.Add(item);
                }

                // Add the separator (if this is not the last one).
                if (groupIndex != grouped.Count() - 1)
                {
                    menu.Items.Add(new ToolStripSeparator());
                }

                groupIndex++;
            }
        }

        /// <summary>
        /// Creates the context menu item.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        private ToolStripItem CreateContextMenuItem(HierarchyTask task)
        {
            ToolStripItem item = null;

            // Separator?
            if (task is HierarchyTaskSeparator)
            {
                item = new ToolStripSeparator();
                return item;
            }

            item = new ToolStripMenuItem();
            item.Text = task.Title;
            item.Image = task.Image;
            item.Enabled = task.Enabled;
            item.Tag = task;
            item.Click += new EventHandler(HierarchyTask_Click);

            return item;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Clear the context menu.
            this.ContextMenuStrip = null;

            // Not the right button?
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            // Perform hit test on the mouse position.
            var hitTestInfo = this.HitTest(e.X, e.Y);
            if (hitTestInfo.Node == null)
            {
                return;
            }

            // Select the node.
            this.SelectedNode = hitTestInfo.Node;

            // Get the hierarhcy info.
            HierarchyNode info = hitTestInfo.Node.Tag as HierarchyNode;
            if (info == null)
            {
                return;
            }

            // Create the context menu.
            ContextMenuStrip menu = this.CreateTaskContextMenuStrip(info);

            // Show the context menu.
            this.ContextMenuStrip = menu;

            // Select the node.
            this.SelectedNode = hitTestInfo.Node;
        }

        /// <summary>
        /// Handles the Click event of the HierarchyTask control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void HierarchyTask_Click(object sender, EventArgs e)
        {
            // Get the tree node.
            var node = sender as ToolStripItem;
            if (node == null)
            {
                return;
            }

            // Get the action.
            HierarchyTask task = node.Tag as HierarchyTask;
            if (task == null || task.Owner == null)
            {
                return;
            }

            // Got an action?
            if (task.Action == null)
            {
                return;
            }

            // Execute the action.
            task.Action(task.Owner);
        }
    }
}
