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
using System.IO;
using System.Xml;

namespace BlackBox.Editor
{
	public partial class XmlEditor : UserControl
	{
        public event EventHandler<LogConfigurationEventArgs> Synchronize = (s, a) => { };

		public XmlEditor()
		{
			// Perform initialization.
			this.InitializeComponent();

			// Initialize the editor.
			_editor.SetHighlighting("XML");
			_editor.Document.FormattingStrategy = new XmlFormattingStrategy();
		}

		public void LoadConfiguration(LogConfiguration configuration)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				configuration.Save(stream);
				stream.Position = 0; // Rewind the stream.
				using (StreamReader reader = new StreamReader(stream))
				{
					_editor.Text = reader.ReadToEnd();
				}
			}
		}

        private void _buttonSynchronize_Click(object sender, EventArgs e)
        {
            try
            {
                LogConfiguration configuration = LogConfiguration.FromXml(_editor.Text);
                this.Synchronize(this, new LogConfigurationEventArgs(configuration));
            }
            catch (BlackBoxException ex)
            {
                string title = "BlackBox Log Configuration Editor";
                string message = "An error occured while deserializing the configuration.\r\n{0}";
                MessageBox.Show(string.Format(message, ex.Message), title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (XmlException ex)
            {
                string title = "BlackBox Log Configuration Editor";
                string message = "The XML is not valid.\r\n\r\n{0}";
                MessageBox.Show(string.Format(message, ex.Message), title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string title = "BlackBox Log Configuration Editor";
                string message = "An unkown error occured.\r\n\r\n{0}";
                MessageBox.Show(string.Format(message, ex.Message), title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
	}
}
