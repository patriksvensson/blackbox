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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace BlackBox.Editor
{
	public partial class AddSinkDialog : Form
	{
		public Type SelectedType
		{
			get
			{
				return (_comboTypes.SelectedItem != null)
					? ((NameValuePair<Type>)_comboTypes.SelectedItem).Value : null;
			}
		}

		public AddSinkDialog()
		{
			// Perform initialization.
			this.InitializeComponent();

			// Create the list of types.
			IEnumerable<Type> types = TypeFinder.FindLogSinks(null);
			foreach (Type type in types)
			{
				NameValuePair<Type> pair = new NameValuePair<Type>(type.FullName, type);
				_comboTypes.Items.Add(pair);
			}

            // Select the first type.
            _comboTypes.SelectedIndex = 0;
		}

		private void _buttonCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void _buttonOK_Click(object sender, EventArgs e)
		{
			if (_comboTypes.SelectedItem != null)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();				
			}
		}
	}
}
