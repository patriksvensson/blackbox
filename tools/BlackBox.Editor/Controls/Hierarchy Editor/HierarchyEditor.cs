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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace BlackBox.Editor
{
    public partial class HierarchyEditor : UserControl
    {
        private ObscuringTypeDescriptor _currentDescriptor;

        public event EventHandler<EventArgs> Dirty = (s, a) => { };

        public HierarchyEditor()
        {
            // Initialize all components.
            this.InitializeComponent();

            // Subscribe to the dirty event from the tree.
            _tree.Dirty += new EventHandler(OnTreeDirty);
        }

        public void LoadConfiguration(LogConfiguration configuration)
        {
            // Create the provider list.
            List<HierarchyNodeProvider> providers = new List<HierarchyNodeProvider>();
            providers.Add(new AssemblyNodeProvider());
            providers.Add(new LogFilterNodeProvider());
            providers.Add(new LogSinkNodeProvider());

            // Create the hierarchy context.
            HierarchyContext context = new HierarchyContext(configuration);

            // Initialize the hierarchy tree.
            _tree.Initialize(context, providers);

            // Build the hierarchy tree.
            _tree.Build();
        }

        private void NodeSelected(object sender, HierarchyNodeEventArgs e)
		{
            // Dispose the current descriptor.
            if (_currentDescriptor != null) {                
                _currentDescriptor.Dispose();
            }

			object obj = e.Node.GetDataObject();
			if (obj is LogSink || obj is LogFilter || obj is Assembly)
			{
                _currentDescriptor = new ObscuringTypeDescriptor(obj);
                _currentDescriptor.PropertyChanged += new PropertyChangedEventHandler(OnPropertyChanged);
				_propertyGrid.SelectedObject = _currentDescriptor;
			}
			else
			{
				_propertyGrid.SelectedObject = null;
			}
		}

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Dirty(this, EventArgs.Empty);
        }

        private void OnTreeDirty(object sender, EventArgs e)
        {
            this.Dirty(this, EventArgs.Empty);
        }
    }
}
