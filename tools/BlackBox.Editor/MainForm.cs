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
using System.Xml.Linq;
using System.IO;

namespace BlackBox.Editor
{
	public partial class MainForm : Form
    {
        #region Private Fields

        private LogConfiguration _configuration;
        private FileInfo _file;
        private bool _isDirty;

        #endregion

        #region Properties

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                _isDirty = value;
                _menuSave.Enabled = _isDirty;
            }
        }

        #endregion

        #region Construction

        public MainForm()
		{
			// Perform initialization.
			this.InitializeComponent();
			this.InitializeApplication();

            // Initially, nothing is dirty.
            this.IsDirty = false;

            // Subscribe to dirty events.
            _hierarchyEditor.Dirty += new EventHandler<EventArgs>(OnDirty);

            // Update the title bar.
            this.UpdateTitleBar();
		}

		#endregion

		#region Initialization

		public void InitializeApplication()
		{
#if DEBUG
			// Load a pre-made configuration.
            // This is for debug purposes only.
			LogConfiguration configuration = new LogConfiguration();			
			FunnelProxy proxy = new FunnelProxy();
			BufferProxy bufferProxy = new BufferProxy();
            bufferProxy.Filters.Add(new ConditionFilter { Condition = "has-exception==false OR level>2", Action = LogFilterResult.Filter });
            bufferProxy.Sinks.Add(new FileSink());
            bufferProxy.Sinks.Add(new MessageQueueSink { CreateIfNotExists = true, Queue = @".\Private$\BlackBox", Label = "$(level(numeric=false))" });
			proxy.Sinks.Add(bufferProxy);
            configuration.Sinks.Add(new TraceSink());
            configuration.Sinks.Add(proxy);
            configuration.Filters.Add(new LevelMatchFilter { Level = LogLevel.Debug, Action = LogFilterResult.Filter });
            configuration.Assemblies.Add(typeof(LogSink).Assembly);
			this.LoadConfiguration(configuration);
#else
			// Load an empty configuration.
			this.LoadConfiguration(null /* Empty configuration */);
#endif
		}

		#endregion

        #region Helper Methods

        public void LoadConfiguration(LogConfiguration configuration)
		{
			_configuration = configuration ?? new LogConfiguration();
			_hierarchyEditor.LoadConfiguration(_configuration);			

			this.IsDirty = false;
			this.UpdateTitleBar();
		}

        public void UpdateTitleBar()
        {
            string applicationName = "BlackBox Log Configuration Editor";
            string filename = _file != null ? _file.Name : "Untitled";
            this.Text = string.Format("{0}{1} - {2}", _isDirty ? "*" : string.Empty, filename, applicationName);            
        }

        public DialogResult ShowSaveQuestion()
        {
            string title = "BlackBox Log Configuration Editor";
            string filename = _file != null ? _file.Name : "Untitled";
            string message = string.Format("Do you want to save changes to '{0}'?", filename);
            return MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        public bool SaveConfiguration(bool showSaveDialog, bool saveAs)
        {
            showSaveDialog = showSaveDialog ? showSaveDialog : _file == null;
            if (showSaveDialog)
            {
                _saveFileDialog.Title = saveAs ? "Save As" : "Save";
                if (_saveFileDialog.ShowDialog() != DialogResult.OK) 
                {
                    return false;
                }
                _file = new FileInfo(_saveFileDialog.FileName);
            }

            // Save the file.
            _configuration.Save(_file);

            // We're not dirty anymore.
            this.IsDirty = false;
            this.UpdateTitleBar();

            // We've saved the configuration.
            return true;
        }

        #endregion

        #region Menu Events

        private void _menuNew_Click(object sender, EventArgs e)
        {
            if (this.IsDirty)
            {
                DialogResult result = this.ShowSaveQuestion();
                if (result == DialogResult.Yes)
                {
                    if (!this.SaveConfiguration(_file == null, false))
                    {
                        return;
                    }
                }
            }

            this.LoadConfiguration(null);
        }

        private void _menuOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = _openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
				try
				{
					// Get the file to load.
					_file = new FileInfo(_openFileDialog.FileName);

					// Parse the configuration.
					LogConfiguration configuration = LogConfiguration.FromXml(_file);

					// Load the configuration in the editor.
					this.LoadConfiguration(configuration);

					// We're not dirty anymore.
					this.IsDirty = false;

					// Update the title.
					this.UpdateTitleBar();
				}
				catch(BlackBoxException exception)
				{
					string title = "BlackBox Log Configuration Editor";
					string message = string.Format("The configuration could not be loaded.\r\n\r\n{0}", exception.Message);
					MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
            }
        }

        private void _menuSave_Click(object sender, EventArgs e)
        {
            this.SaveConfiguration(false, false);
        }

        private void _menuSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveConfiguration(true, true);
        }

        private void _menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _menuAbout_Click(object sender, EventArgs e)
        {
            using (AboutDialog dialog = new AboutDialog())
            {
                dialog.ShowDialog();
            }
        }

        #endregion

        private void OnTabPageSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tabControl.SelectedTab == _tabPageXml)
            {
                // Load the configuration in the XML editor.
                _xmlEditor.LoadConfiguration(_configuration);
            }
        }

        private void OnXmlSynchronize(object sender, LogConfigurationEventArgs e)
        {
            // Load the synchronized configuration.
			this.LoadConfiguration(e.Configuration);
			
			// We're dirty.
			this.IsDirty = true;

			// Update the title.
			this.UpdateTitleBar();

			// Switch to the hierarchy tab.
			_tabControl.SelectedTab = _tabPageHierarchy;			
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (this.IsDirty)
                {
                    DialogResult result = this.ShowSaveQuestion();
                    if (result == DialogResult.Yes)
                    {
                        if (!this.SaveConfiguration(_file == null, false))
                        {
                            e.Cancel = true;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void OnDirty(object sender, EventArgs e)
        {
            this.IsDirty = true;
            this.UpdateTitleBar();
        }
	}
}